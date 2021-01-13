using System;
using System.Collections.Generic;

namespace ReduxSharp.Activation.IOC
{
    public class TypeRequest : ITypeRequest
    {
        public Type Type { get; }

        /// <summary>
        /// Gets what this binding should be resolved as
        /// </summary>
        public Type BindAs { get; }

        public List<IParameter> Parameters { get; init; }

        public TypeRequest(Type type)
        {
            Type = type;
            BindAs = type;
            Parameters = new List<IParameter>();
        }

        public TypeRequest(Type type, Type bindAs)
        {
            Type = type;
            BindAs = bindAs;
            Parameters = new List<IParameter>();
        }


        public TypeRequest(Type type, params IParameter[] parameters)
        {
            Type = type;
            BindAs = type;
            Parameters = new List<IParameter>(parameters);
        }

        public TypeRequest(Type type, Type bindAs, params IParameter[] parameters)
        {
            Type = type;
            BindAs = bindAs;
            Parameters = new List<IParameter>(parameters);
        }

        IReadOnlyList<IParameter> ITypeRequest.Parameters
            => Parameters;
    }
}
