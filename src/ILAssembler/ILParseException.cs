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

        internal ILParseException(ParseError[] errors) : base(errors)
        {
        }

        internal ILParseException(ParseError[] errors, ExceptionDispatchInfo sourceException) : base(errors)
        {
            SourceExceptionInfo = sourceException ?? throw new ArgumentNullException(nameof(sourceException));
        }

        internal static ILParseException Create(IScriptExtent extent, string errorId, ExceptionDispatchInfo inner)
        {
            return new ILParseException(
                new[]
                {
                    new ParseError(extent, errorId, inner.SourceException.Message)
                },
                inner);
        }

        internal static ILParseException Create(IScriptExtent extent, string errorId, string message, ExceptionDispatchInfo inner)
        {
            return new ILParseException(
                new[]
                {
                    new ParseError(extent, errorId, message)
                },
                inner);
        }

        internal static ILParseException Create(IScriptExtent extent, string errorId, string message)
        {
            return new ILParseException(
                new[]
                {
                    new ParseError(extent, errorId, message)
                });
        }

        public ExceptionDispatchInfo? SourceExceptionInfo { get; }
    }
}
