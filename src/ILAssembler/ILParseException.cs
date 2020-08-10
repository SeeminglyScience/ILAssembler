using System;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.ExceptionServices;

namespace ILAssembler
{
    public sealed class ILParseException : ParseException
    {
        private ILParseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public ILParseException()
        {
        }

        public ILParseException(string message) : base(message)
        {
        }

        public ILParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ILParseException(ParseError[] errors) : base(errors)
        {
        }

        public ILParseException(ParseError[] errors, ExceptionDispatchInfo sourceException) : base(errors)
        {
            SourceExceptionInfo = sourceException ?? throw new ArgumentNullException(nameof(sourceException));
        }

        public ExceptionDispatchInfo? SourceExceptionInfo { get; }
    }
}
