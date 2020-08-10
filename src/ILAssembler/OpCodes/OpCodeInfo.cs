using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal abstract class OpCodeInfo
    {
        public abstract string Name { get; }

        public abstract ILOpCode OpCode { get; }

        public abstract void Emit(CilAssemblyContext context, CommandAst ast);

        public static OpCodeInfo NoOperand(string name, ILOpCode opCode)
        {
            return new NoOperandOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo AutoLocalOperand(string name, bool isSet, bool isAddress)
        {
            return new AutoLocalOpCodeInfo(name, isSet, isAddress);
        }

        public static OpCodeInfo BranchOperand(string name, ILOpCode opCode)
        {
            return new BranchOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo StringOperand(string name, ILOpCode opCode)
        {
            return new StringOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo ByteOperand(string name, ILOpCode opCode)
        {
            return new ByteOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo Int32Operand(string name, ILOpCode opCode)
        {
            return new Int32OpCodeInfo(name, opCode);
        }

        public static OpCodeInfo Int64Operand(string name, ILOpCode opCode)
        {
            return new Int64OpCodeInfo(name, opCode);
        }

        public static OpCodeInfo SingleOperand(string name, ILOpCode opCode)
        {
            return new SingleOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo DoubleOperand(string name, ILOpCode opCode)
        {
            return new DoubleOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo TypeOperand(string name, ILOpCode opCode)
        {
            return new TypeOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo FieldOperand(string name, ILOpCode opCode)
        {
            return new FieldOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo MethodOperand(string name, ILOpCode opCode)
        {
            return new MethodOpCodeInfo(name, opCode);
        }

        public static OpCodeInfo ConstructorOperand(string name, ILOpCode opCode)
        {
            return new ConstructorOpCodeInfo(name, opCode);
        }
    }
}
