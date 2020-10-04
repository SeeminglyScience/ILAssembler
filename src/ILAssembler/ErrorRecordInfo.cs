using System.Management.Automation;

namespace ILAssembler
{
    internal sealed class ErrorRecordInfo
    {
        public ErrorRecordInfo(string id, ErrorCategory category, object? targetObject)
        {
            Id = id;
            Category = category;
            TargetObject = targetObject;
        }

        public string Id { get; }

        public ErrorCategory Category { get; }

        public object? TargetObject { get; }
    }
}
