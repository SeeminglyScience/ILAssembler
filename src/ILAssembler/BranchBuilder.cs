using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace ILAssembler
{
    internal sealed class BranchBuilder
    {
        private readonly InstructionEncoder _encoder;

        private List<LabelRef>? _references;

        private Dictionary<LabelHandle, int>? _marks;

        private Dictionary<string, LabelHandle>? _labels;

        private List<ExceptionHandlerInfo>? _lazyExceptionHandlers;

        private int _nextLabelId;

        public BranchBuilder(InstructionEncoder encoder)
        {
            _encoder = encoder;
        }

        public void AddExceptionRegion(
            ExceptionRegionKind kind,
            LabelHandle tryStart,
            LabelHandle tryEnd,
            LabelHandle handlerStart,
            LabelHandle handlerEnd,
            LabelHandle filterStart = default,
            EntityHandle catchType = default)
        {
            (_lazyExceptionHandlers ??= new()).Add(
                new ExceptionHandlerInfo(
                    kind,
                    tryStart,
                    tryEnd,
                    handlerStart,
                    handlerEnd,
                    filterStart,
                    catchType));
        }

        public LabelHandle DefineLabel()
        {
            return new LabelHandle(Interlocked.Increment(ref _nextLabelId));
        }

        public void Branch(ILOpCode opCode, LabelHandle label)
        {
            bool isShort = opCode.GetBranchOperandSize() == 1;
            _encoder.OpCode(opCode);
            int offset = _encoder.Offset;
            if (isShort)
            {
                _encoder.CodeBuilder.WriteSByte(-1);
            }
            else
            {
                _encoder.CodeBuilder.WriteInt32(-1);
            }

            (_references ??= new()).Add(
                new LabelRef(
                    offset,
                    _encoder.Offset,
                    label,
                    isShort));
        }

        public void Switch(LabelHandle[] labels)
        {
            _encoder.OpCode(ILOpCode.Switch);
            _encoder.CodeBuilder.WriteUInt32((uint)labels.Length);
            Span<int> offsets = stackalloc int[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                offsets[i] = _encoder.Offset;
                _encoder.CodeBuilder.WriteInt32(-1);
            }

            var afterSwitch = _encoder.Offset;
            _references ??= new();
            for (int i = 0; i < labels.Length; i++)
            {
                _references.Add(
                    new LabelRef(
                        offsets[i],
                        afterSwitch,
                        labels[i],
                        isShort: false));
            }
        }

        internal void SerializeExceptionTable(BlobBuilder builder)
        {
            bool HasSmallExceptionRegions()
            {
                if (!ExceptionRegionEncoder.IsSmallRegionCount(_lazyExceptionHandlers.Count))
                {
                    return false;
                }

                foreach (var handler in _lazyExceptionHandlers)
                {
                    int tryStartOffset = GetLabelOffsetChecked(handler.TryStart);
                    int tryEndOffset = GetLabelOffsetChecked(handler.TryEnd);
                    int handlerStartOffset = GetLabelOffsetChecked(handler.HandlerStart);
                    int handlerEndOffset = GetLabelOffsetChecked(handler.HandlerEnd);

                    bool isTrySmallRegion = ExceptionRegionEncoder.IsSmallExceptionRegion(
                        tryStartOffset,
                        tryEndOffset - tryStartOffset);

                    if (!isTrySmallRegion)
                    {
                        return false;
                    }

                    bool isHandlerSmallRegion = ExceptionRegionEncoder.IsSmallExceptionRegion(
                        handlerStartOffset,
                        handlerEndOffset - handlerStartOffset);

                    if (!isHandlerSmallRegion)
                    {
                        return false;
                    }
                }

                return true;
            }

            if (_lazyExceptionHandlers == null || _lazyExceptionHandlers.Count == 0)
            {
                return;
            }

            bool hasSmallFormat = HasSmallExceptionRegions();
            SerializeExceptionTableHeader(builder, _lazyExceptionHandlers.Count, hasSmallFormat);

            foreach (var handler in _lazyExceptionHandlers)
            {
                // Note that labels have been validated when added to the handler list,
                // they might not have been marked though.

                int tryStart = GetLabelOffsetChecked(handler.TryStart);
                int tryEnd = GetLabelOffsetChecked(handler.TryEnd);
                int handlerStart = GetLabelOffsetChecked(handler.HandlerStart);
                int handlerEnd = GetLabelOffsetChecked(handler.HandlerEnd);

                int catchTokenOrOffset = handler.Kind switch
                {
                    ExceptionRegionKind.Catch => MetadataTokens.GetToken(handler.CatchType),
                    ExceptionRegionKind.Filter => GetLabelOffsetChecked(handler.FilterStart),
                    _ => 0,
                };

                if (hasSmallFormat)
                {
                    builder.WriteUInt16((ushort)handler.Kind);
                    builder.WriteUInt16((ushort)tryStart);
                    builder.WriteByte((byte)(tryEnd - tryStart));
                    builder.WriteUInt16((ushort)handlerStart);
                    builder.WriteByte((byte)(handlerEnd - handlerStart));
                }
                else
                {
                    builder.WriteInt32((int)handler.Kind);
                    builder.WriteInt32(tryStart);
                    builder.WriteInt32(tryEnd - tryStart);
                    builder.WriteInt32(handlerStart);
                    builder.WriteInt32(handlerEnd - handlerStart);
                }

                builder.WriteInt32(catchTokenOrOffset);
            }
        }

        private static void SerializeExceptionTableHeader(BlobBuilder builder, int exceptionRegionCount, bool hasSmallRegions)
        {
            const int TableHeaderSize = 4;

            const int SmallRegionSize =
                sizeof(short) +  // Flags
                sizeof(short) +  // TryOffset
                sizeof(byte) +   // TryLength
                sizeof(short) +  // HandlerOffset
                sizeof(byte) +   // HandleLength
                sizeof(int);     // ClassToken | FilterOffset

            const int FatRegionSize =
                sizeof(int) +    // Flags
                sizeof(int) +    // TryOffset
                sizeof(int) +    // TryLength
                sizeof(int) +    // HandlerOffset
                sizeof(int) +    // HandleLength
                sizeof(int);     // ClassToken | FilterOffset

            static int GetExceptionTableSize(int exceptionRegionCount, bool isSmallFormat)
            {
                return TableHeaderSize + (exceptionRegionCount * (isSmallFormat ? SmallRegionSize : FatRegionSize));
            }

            const byte EHTableFlag = 0x01;
            const byte FatFormatFlag = 0x40;

            bool hasSmallFormat = hasSmallRegions && ExceptionRegionEncoder.IsSmallRegionCount(exceptionRegionCount);
            int dataSize = GetExceptionTableSize(exceptionRegionCount, hasSmallFormat);

            builder.Align(4);
            if (hasSmallFormat)
            {
                builder.WriteByte(EHTableFlag);
                builder.WriteByte(unchecked((byte)dataSize));
                builder.WriteInt16(0);
            }
            else
            {
                builder.WriteByte(EHTableFlag | FatFormatFlag);
                builder.WriteByte(unchecked((byte)dataSize));
                builder.WriteUInt16(unchecked((ushort)(dataSize >> 8)));
            }
        }

        internal void FixupCodeBuilder(BlobBuilder destination)
        {
            if (_references is null || _references.Count == 0)
            {
                _encoder.CodeBuilder.WriteContentTo(destination);
                return;
            }

            LabelRef[] references = _references.ToArray();
            LabelRef reference = references[0];
            int branchIndex = 0;

            // offset within the source builder
            int srcOffset = 0;

            // current offset within the current source blob
            int srcBlobOffset = 0;

            foreach (Blob srcBlob in _encoder.CodeBuilder.GetBlobs())
            {
                while (true)
                {
                    // copy bytes preceding the next branch, or till the end of the blob:
                    int chunkSize = Math.Min(reference.UsageOffset - srcOffset, srcBlob.Length - srcBlobOffset);
                    destination.WriteBytes(srcBlob.GetBytes().Array!, srcBlobOffset, chunkSize);
                    srcOffset += chunkSize;
                    srcBlobOffset += chunkSize;

                    // there is no branch left in the blob:
                    if (srcBlobOffset == srcBlob.Length)
                    {
                        srcBlobOffset = 0;
                        break;
                    }

                    bool isShortInstruction = reference.IsShort;
                    int branchDistance = GetLabelOffsetChecked(reference.Label) - reference.CalculateFrom;

                    // write branch operand:
                    if (isShortInstruction)
                    {
                        destination.WriteSByte((sbyte)branchDistance);
                    }
                    else
                    {
                        destination.WriteInt32(branchDistance);
                    }

                    srcOffset += isShortInstruction ? 1 : 4;

                    // next branch:
                    branchIndex++;
                    if (branchIndex == _references.Count)
                    {
                        reference = new LabelRef(int.MaxValue, int.MaxValue, label: default, isShort: default);
                    }
                    else
                    {
                        reference = references[branchIndex];
                    }

                    // the branch starts at the very end and its operand is in the next blob:
                    if (srcBlobOffset == srcBlob.Length - 1)
                    {
                        srcBlobOffset = isShortInstruction ? 1 : 4;
                        break;
                    }

                    // skip fake branch instruction:
                    srcBlobOffset += isShortInstruction ? 1 : 4;
                }
            }
        }

        public void MarkLabel(LabelHandle label)
        {
            _marks ??= new();
            if (_marks.ContainsKey(label))
            {
                throw new InvalidOperationException("Label already marked.");
            }

            _marks.Add(label, _encoder.Offset);
        }

        public LabelHandle GetOrCreateLabel(string name)
        {
            if (_labels is not null && _labels.TryGetValue(name, out LabelHandle handle))
            {
                return handle;
            }

            LabelHandle label = DefineLabel();
            (_labels ??= new()).Add(name, label);
            return label;
        }

        private int GetLabelOffsetChecked(LabelHandle label)
        {
            int offset = 0;
            if (_marks is null || !_marks.TryGetValue(label, out offset))
            {
                Debug.Assert(
                    _labels is not null,
                    "GetLabelOffsetChecked should not be called if no labels were created.");

                string? labelName = null;
                foreach (var kvp in _labels)
                {
                    if (kvp.Value.Equals(label))
                    {
                        labelName = kvp.Key;
                    }
                }

                Throw.InvalidOperationException(
                    SR.Format(
                        SR.LabelNotMarked,
                        (object?)labelName ?? label.Value));
            }

            return offset;
        }

        internal readonly struct ExceptionHandlerInfo
        {
            public readonly ExceptionRegionKind Kind;

            public readonly LabelHandle TryStart, TryEnd, HandlerStart, HandlerEnd, FilterStart;

            public readonly EntityHandle CatchType;

            public ExceptionHandlerInfo(
                ExceptionRegionKind kind,
                LabelHandle tryStart,
                LabelHandle tryEnd,
                LabelHandle handlerStart,
                LabelHandle handlerEnd,
                LabelHandle filterStart,
                EntityHandle catchType)
            {
                Kind = kind;
                TryStart = tryStart;
                TryEnd = tryEnd;
                HandlerStart = handlerStart;
                HandlerEnd = handlerEnd;
                FilterStart = filterStart;
                CatchType = catchType;
            }
        }

        private readonly struct LabelRef : IEquatable<LabelRef>
        {
            public readonly int UsageOffset;

            public readonly int CalculateFrom;

            public readonly LabelHandle Label;

            public readonly bool IsShort;

            public LabelRef(int usageOffset, int calculateFrom, LabelHandle label, bool isShort)
            {
                UsageOffset = usageOffset;
                CalculateFrom = calculateFrom;
                Label = label;
                IsShort = isShort;
            }

            public bool Equals(LabelRef other)
            {
                return UsageOffset == other.UsageOffset
                    && CalculateFrom == other.CalculateFrom
                    && Label.Equals(other.Label)
                    && IsShort == other.IsShort;
            }

            public override bool Equals(object? obj) => obj is LabelRef other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(UsageOffset, CalculateFrom, Label, IsShort);
        }
    }

    internal readonly struct LabelHandle : IEquatable<LabelHandle>
    {
        public readonly int Value;

        public LabelHandle(int value) => Value = value;

        public static bool operator ==(LabelHandle left, LabelHandle right) => left.Equals(right);

        public static bool operator !=(LabelHandle left, LabelHandle right) => !left.Equals(right);

        public bool Equals(LabelHandle other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is LabelHandle other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();
    }
}
