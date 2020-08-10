using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler
{
    internal sealed class CilAssemblyContext
    {
        private InstructionEncoder _encoder;

        public CilAssemblyContext(DynamicILInfo ilInfo)
        {
            _encoder = new InstructionEncoder(
                new BlobBuilder(),
                new ControlFlowBuilder());

            ILInfo = ilInfo;
            Branches = new Dictionary<string, LabelHandle>(StringComparer.Ordinal);
        }

        public DynamicILInfo ILInfo { get; }

        public Dictionary<string, LabelHandle> Branches { get; }

        public string[]? Locals { get; set; }

        public ref InstructionEncoder Encoder => ref _encoder;

        public LabelHandle GetOrAddLabel(string name)
        {
            if (Branches.TryGetValue(name, out LabelHandle existingHandle))
            {
                return existingHandle;
            }

            var handle = Encoder.DefineLabel();
            Branches.Add(name, handle);
            return handle;
        }
    }
}
