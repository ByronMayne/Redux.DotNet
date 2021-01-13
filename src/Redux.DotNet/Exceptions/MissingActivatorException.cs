using System;
using System.Collections.Generic;
using System.Linq;

namespace ReduxSharp.Exceptions
{
    /// <summary>
    /// Exception thorwn when trying to configure the store using <see cref="StoreConfiguration{TState}"/> and
    /// they are missing an activator
    /// </summary>
    public class MissingActivatorException : ReduxSharpException
    {
        public MissingActivatorException() : base(FormatMessage())
        {
        }

        private static string FormatMessage()
            => $"No activator has been configured for Redux DotNet you must provide you own using .UsingActivtor or use .UseDefaultActivator`";
    }
}