using System;
using System.Collections.Generic;
using System.Text;

namespace Redux.DotNet.Exceptions
{
    public class MissingCopyConstructorException : ReduxSharpException
    {
        public Type Type { get; }
        public MissingCopyConstructorException(Type type) : base(FormatMessage(type))
        {
        }

        private static string FormatMessage(Type type)
            => $"The type {type.FullName} is missing the required copy constructor. Please define a public or" +
            $" private constructor for it with the follwing siginture `{type.Name}({type.Name} instance)`";
    }
}
