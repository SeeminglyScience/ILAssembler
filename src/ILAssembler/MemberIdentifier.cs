using System.Management.Automation.Language;
using System.Reflection;

namespace ILAssembler
{
    internal abstract class MemberIdentifier
    {
        protected MemberIdentifier(TypedIdentifier? declaringType, string name, bool isStatic)
        {
            DeclaringType = declaringType;
            Name = name;
            IsStatic = isStatic;
        }

        public TypedIdentifier? DeclaringType { get; }

        public string Name { get; }

        public bool IsStatic { get; }

        public abstract int GetToken(CilAssemblyContext context, IScriptExtent subject);

        protected virtual ILParseException ErrorMemberNotFound(IScriptExtent subject)
        {
            return Error.Parse(
                subject,
                nameof(Strings.MemberNotFound),
                Strings.MemberNotFound);
        }

        protected virtual BindingFlags GetFlags()
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public;
            flags |= IsStatic ? BindingFlags.Static : BindingFlags.Instance;
            return flags;
        }
    }
}
