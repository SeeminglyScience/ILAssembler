using System.Diagnostics;
using System.Management.Automation.Language;
using System.Reflection;

namespace ILAssembler
{
    internal class FieldIdentifier : MemberIdentifier
    {
        public TypedIdentifier FieldType { get; }

        public FieldIdentifier(TypedIdentifier declaringType, string name, bool isStatic, TypedIdentifier fieldType)
            : base(declaringType, name, isStatic)
        {
            FieldType = fieldType;
        }

        public override int GetToken(CilAssemblyContext context, IScriptExtent subject)
        {
            Debug.Assert(
                DeclaringType is not null,
                "FieldSignatureParser.GetMemberIdentifier should have thrown when declaring type is null.");

            FieldInfo? field = DeclaringType.Type.GetField(
                Name,
                GetFlags());

            if (field is null)
            {
                throw ErrorMemberNotFound(subject);
            }

            if (field.DeclaringType!.IsGenericType)
            {
                return context.ILInfo.GetTokenFor(
                    field.FieldHandle,
                    field.DeclaringType.TypeHandle);
            }

            return context.ILInfo.GetTokenFor(field.FieldHandle);
        }
    }
}
