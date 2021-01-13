using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxSharp.Exceptions
{
    public class InvalidMiddlewareException : ReduxSharpException
    {
        public InvalidMiddlewareException(Type middlewareType) : base(FormatMessage(middlewareType))
        {

        }

        private static string FormatMessage(Type type)
        {
            return $"The middleware type {type.FullName} is invalid. You can only use untyped or one that matches the store type";
        }
    }
}
