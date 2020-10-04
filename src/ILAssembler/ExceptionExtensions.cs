using System;
using System.Management.Automation;

namespace ILAssembler
{
    internal static class ExceptionExtensions
    {
        public static TException WithErrorInfo<TException>(
            this TException exception,
            string? id = null,
            ErrorCategory category = ErrorCategory.NotSpecified,
            object? targetObject = null)
            where TException : Exception
        {
            exception.Data[typeof(ErrorRecordInfo)] = new ErrorRecordInfo(
                id ?? typeof(TException).Name,
                category,
                targetObject);

            return exception;
        }

        public static bool TryCreateErrorRecord(this Exception exception, out ErrorRecord errorRecord)
        {
            object? data = exception.Data[typeof(ErrorRecordInfo)];
            if (data is not ErrorRecordInfo info)
            {
                errorRecord = null!;
                return false;
            }

            errorRecord = new ErrorRecord(
                exception,
                info.Id,
                info.Category,
                info.TargetObject);
            return true;
        }
    }
}
