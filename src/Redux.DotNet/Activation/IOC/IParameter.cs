using System;

namespace ReduxSharp.Activation.IOC
{
    /// <summary>
    /// Modifies an activation process in some way.
    /// </summary>
    public interface IParameter : IEquatable<IParameter>
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the parameter type
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the value for the parameter within the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <returns>The value for the parameter.</returns>
        object GetValue();
    }
}
