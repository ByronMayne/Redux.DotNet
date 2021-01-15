using System;
using System.Runtime.Serialization;

namespace Redux.DotNet.Exceptions
{
    public class ReduxSharpException : Exception
    {
        public ReduxSharpException()
        {
        }

        public ReduxSharpException(string message) : base(message)
        {
        }

        public ReduxSharpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReduxSharpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
