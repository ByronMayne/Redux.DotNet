using ReduxSharp.Activation.IOC;
using ReduxSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReduxSharp.Activation
{
    /// <summary>
    /// The default activator that is used by Redux Sharp. 
    /// </summary>
    public class DefaultActivator : IActivator
    {
        public DefaultActivator()
        { }

        /// <inheritdoc cref="IActivator"/>
        T IActivator.Get<T>(ITypeRequest typeRequest, IEnumerable<IParameter> addtionalParameters = null)
            => Get<T>(typeRequest, addtionalParameters);

        /// <inheritdoc cref="IActivator"/>
        object IActivator.Get(ITypeRequest typeRequest, IEnumerable<IParameter> addtionalParameters = null)
            => Get(typeRequest, addtionalParameters);

        /// <summary>
        /// The default activation implemention that can be overridden by the end users. It is however always
        /// used at least once to create the first activator.
        /// </summary>
        /// <param name="type">The type wanting to be created</param>
        /// <param name="activationInfo">The type activation info</param>
        /// <returns>The created instance</returns>
        internal static T Get<T>(ITypeRequest typeRequest, IEnumerable<IParameter> addtionalParameters = null)
            => (T)Get(typeRequest, addtionalParameters);

        /// <summary>
        /// The default activation implemention that can be overridden by the end users. It is however always
        /// used at least once to create the first activator.
        /// </summary>
        /// <param name="type">The type wanting to be created</param>
        /// <param name="activationInfo">The type activation info</param>
        /// <returns>The created instance</returns>
        internal static object Get(ITypeRequest typeRequest, IEnumerable<IParameter> addtionalParameters = null)
        {
            if (typeRequest == null) throw new ArgumentNullException(nameof(typeRequest));

            List<IParameter> parameters = new List<IParameter>(typeRequest.Parameters);

            if (addtionalParameters != null)
            {
                parameters.AddRange(addtionalParameters);
            }

            object instance = Construct(typeRequest.Type, parameters);

            return instance;
        }

        private static object Construct(Type type, IReadOnlyList<IParameter> addtionalParameters = null)
        {
            if (addtionalParameters == null)
                addtionalParameters = Array.Empty<IParameter>();

            ConstructorInfo[] orderedConstructors = type
                .GetConstructors()
                .OrderBy(c => c.GetParameters())
                .ToArray();

            foreach (ConstructorInfo constructor in orderedConstructors)
            {
                if (TryConstruct(constructor, addtionalParameters, out object instance))
                {
                    return instance;
                }
            }

            throw new ConstructionException(type, addtionalParameters);
        }

        /// <summary>
        /// Attempst to construct an instance of a type using the supplied parameters.
        /// </summary>
        /// <param name="constructor">The constructor to try</param>
        /// <param name="parameters">The parameters to use</param>
        /// <param name="instance">The instance that was created</param>
        /// <returns>True if one could be created otherwise false</returns>
        private static bool TryConstruct(ConstructorInfo constructor, IReadOnlyList<IParameter> parameters, out object instance)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            instance = null;

            ParameterInfo[] parameterInfos = constructor.GetParameters();

            object[] arguments = new object[parameterInfos.Length];

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                bool wasResolved = false;
                ParameterInfo parameterInfo = parameterInfos[i];

                foreach (IParameter parameter in parameters)
                {
                    // Check the name 
                    if (string.Equals(parameter.Name, parameterInfo.Name))
                    {
                        arguments[i] = parameter.GetValue();
                        wasResolved = true;
                    }

                    // We only compare types 
                    if (parameter.Type == parameterInfo.ParameterType)
                    {
                        arguments[i] = parameter.GetValue();
                        wasResolved = true;
                    }
                }

                if (!wasResolved)
                {
                    return false;
                }
            }

            instance = constructor.Invoke(arguments);
            return true;
        }
    }
}
