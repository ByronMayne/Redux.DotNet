using ReduxSharp.Activation.IOC;
using System.Collections.Generic;

namespace ReduxSharp.Activation
{
    /// <summary>
    /// Used to create instances of objects at startup
    /// </summary>
    public interface IActivator
    {
        /// <summary>
        /// Attempts to activate a type using the given type request casting back the result
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <param name="typeRequest">The request to resolve</param>
        /// <returns>The result of the request casted as the given type</returns>
        T Get<T>(ITypeRequest typeRequest, IEnumerable<IParameter> addtionalParameters = null);

        /// <summary>
        /// Attempts to activate a type using the given type request casting back the result
        /// </summary>
        /// <param name="typeRequest">The request to resolve</param>
        /// <returns>The result of the request</returns>
        object Get(ITypeRequest typeRequest, IEnumerable<IParameter> addtionalParameters = null);
    }
}
