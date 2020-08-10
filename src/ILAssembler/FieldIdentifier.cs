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
            if (DeclaringType?.Type is null)
            {
                throw subject.GetParseError(
                    "MissingDeclaringType",
                    "Unable to determine the declaring type.");
            }

            FieldInfo? field = DeclaringType.Type.GetField(
                Name,
                GetFlags());

            if (field is null)
            {
                throw ErrorMemberNotFound(subject);
            }

            return context.ILInfo.GetTokenFor(field.FieldHandle);
        }
    }
}
