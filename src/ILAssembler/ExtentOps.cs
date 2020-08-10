using System.Management.Automation.Language;

namespace ILAssembler
{
    internal static class ExtentOps
    {
        public static IScriptExtent ExtentOf(IScriptExtent start, IScriptExtent end)
        {
            if (start == end)
            {
                return start;
            }

            var fullScript = start.StartScriptPosition.GetFullScript();
            var newStart = new ScriptPosition(
                start.File,
                start.StartLineNumber,
                start.StartColumnNumber,
                start.StartScriptPosition.Line,
                fullScript);

            var newEnd = new ScriptPosition(
                end.File,
                end.EndLineNumber,
                end.EndColumnNumber,
                end.EndScriptPosition.Line,
                fullScript);

            return new ScriptExtent(newStart, newEnd);
        }
    }
}
