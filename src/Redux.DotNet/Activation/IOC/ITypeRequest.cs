using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxSharp.Activation.IOC
{
    public interface ITypeRequest
    {
        /// <summary>
        /// Gets the type that is being requested
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the list of parameters for the type
        /// </summary>
        public IReadOnlyList<IParameter> Parameters { get; }
    }
}
