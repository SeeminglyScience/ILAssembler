using System;
using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal sealed class AutoLocalOpCodeInfo : VariableExpressionOpCodeInfo
    {
        private readonly ILOpCode[] _simpleMap;

        public AutoLocalOpCodeInfo(string name, bool isSet, bool isAddress)
            : base(name, GetShortOpCode(isSet, isAddress), GetLongOpCode(isSet, isAddress))
        {
            if (isAddress)
            {
                _simpleMap = Array.Empty<ILOpCode>();
                return;
            }

            if (isSet)
            {
                _simpleMap = new[]
                {
                    ILOpCode.Stloc_0,
                    ILOpCode.Stloc_1,
                    ILOpCode.Stloc_2,
                    ILOpCode.Stloc_3,
                };

                return;
            }

            _simpleMap = new[]
            {
                ILOpCode.Ldloc_0,
                ILOpCode.Ldloc_1,
                ILOpCode.Ldloc_2,
                ILOpCode.Ldloc_3,
            };
        }

        private static ILOpCode GetShortOpCode(bool isSet, bool isAddress)
        {
            if (isSet)
            {
                return ILOpCode.Stloc_s;
            }

            if (isAddress)
            {
                return ILOpCode.Ldloca_s;
            }

            return ILOpCode.Ldloc_s;
        }

        private static ILOpCode GetLongOpCode(bool isSet, bool isAddress)
        {
            if (isSet)
            {
                return ILOpCode.Stloc;
            }

            if (isAddress)
            {
                return ILOpCode.Ldloca;
            }

            return ILOpCode.Ldloc;
        }

        protected override int GetIndex(CilAssemblyContext context, VariableExpressionAst variable)
        {
            int index = context.Locals is null
                ? -1
                : Array.IndexOf(context.Locals, variable.VariablePath.UserPath);
            if (index == -1)
            {
                Throw.ParseException(
                    variable.Extent,
                    nameof(SR.UndefinedLocal),
                    SR.UndefinedLocal);
            }

            return index;
        }

        protected override bool TryGetSimpleOpCode(int index, out ILOpCode opCode)
        {
            if (_simpleMap.Length > index)
            {
                opCode = _simpleMap[index];
                return true;
            }

            opCode = default;
            return false;
        }
    }
}
