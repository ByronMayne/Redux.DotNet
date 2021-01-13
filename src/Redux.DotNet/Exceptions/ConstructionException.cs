using ReduxSharp.Activation.IOC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReduxSharp.Exceptions
{
    public class ConstructionException : ReduxSharpException
    {
        public ConstructionException(Type type, IEnumerable<IParameter> parameters) : base(FormatMessage(type, parameters))
        {
        }

        private static string FormatMessage(Type type, IEnumerable<IParameter> parameters)
        {
            return $"Unable to construct type {type.FullName} as no constructor could be invoked using the following paramter types {string.Join(", ", parameters.Select(p => p.Type.FullName))}.";
        }

    }
}
