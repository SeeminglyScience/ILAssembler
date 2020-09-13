using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal static class Throw
    {
        [DoesNotReturn]
        public static void CannotReadCommandName(IScriptExtent extent)
        {
            throw Error.Parse(extent, nameof(Strings.CannotReadCommandName), Strings.CannotReadCommandName);
        }
    }
}
