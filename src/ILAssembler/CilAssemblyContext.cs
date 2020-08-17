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
            _encoder = new InstructionEncoder(new BlobBuilder());
            BranchBuilder = new BranchBuilder(_encoder);
            ILInfo = ilInfo;
        }

        public DynamicILInfo ILInfo { get; }

        public BranchBuilder BranchBuilder { get; }

        public string[]? Locals { get; set; }

        public ref InstructionEncoder Encoder => ref _encoder;

        public LabelHandle GetOrAddLabel(string name)
        {
            return BranchBuilder.GetOrCreateLabel(name);
        }
    }
}
