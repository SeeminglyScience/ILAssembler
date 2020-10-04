using System.Management.Automation;

namespace ILAssembler
{
    internal record ErrorRecordInfo(string Id, ErrorCategory Category, object? TargetObject);
}
