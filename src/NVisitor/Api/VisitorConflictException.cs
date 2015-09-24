using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace NVisitor.Api
{
    [Serializable]
    public class VisitorConflictException : Exception
    {
        [StringFormatMethod("format")]
        public VisitorConflictException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        protected VisitorConflictException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}