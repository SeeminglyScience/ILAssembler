using System;
using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class AnyTokenOpCodeInfo : TokenOpCodeInfo
    {
        private readonly MethodOpCodeInfo _methodOpCodeInfo;

        private readonly ConstructorOpCodeInfo _ctorOpCodeInfo;

        private readonly FieldOpCodeInfo _fieldOpCodeInfo;

        private readonly TypeOpCodeInfo _typeOpCodeInfo;

        public AnyTokenOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
            _methodOpCodeInfo = new(name, opCode);
            _ctorOpCodeInfo = new(name, opCode);
            _fieldOpCodeInfo = new(name, opCode);
            _typeOpCodeInfo = new(name, opCode);
        }

        protected override string ExpectedSignatureErrorId => "ExpectedSignature";

        protected override string ExpectedSignatureMessage => "Expected type, field, method, or constructor signature.";

        protected override int GetToken(CilAssemblyContext context, ScriptBlockExpressionAst signatureBody)
        {
            var memberTypeAst = signatureBody.ScriptBlock.Visit(new FindMemberTypeAstVisitor());
            if (memberTypeAst is null)
            {
                throw ErrorExpectedSignature(signatureBody);
            }

            if (memberTypeAst is InvokeMemberExpressionAst invoke)
            {
                if (invoke.Member is StringConstantExpressionAst stringConst
                    && !invoke.Static
                    && stringConst.Value.Equals("new", StringComparison.Ordinal))
                {
                    return FallBackGetToken(_ctorOpCodeInfo, context, signatureBody);
                }

                return FallBackGetToken(_methodOpCodeInfo, context, signatureBody);
            }

            if (memberTypeAst is MemberExpressionAst)
            {
                return FallBackGetToken(_fieldOpCodeInfo, context, signatureBody);
            }

            if (memberTypeAst is TypeExpressionAst)
            {
                return FallBackGetToken(_typeOpCodeInfo, context, signatureBody);
            }

            throw ErrorExpectedSignature(signatureBody);
        }
    }
}
