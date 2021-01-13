using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxSharp.Activation.IOC
{
    class FactoryParameter<T> : IParameter
    {
        public string Name { get; }

        public Func<T> Factory { get; }

        public Type Type { get; }

        public FactoryParameter(string name, Func<T> factory)
        {
            Name = name;
            Factory = factory;
            Type = typeof(T);
        }

        public bool Equals(IParameter other)
            => string.Equals(Name, other.Name) && other is FactoryParameter<T>;


        public object GetValue()
            => Factory();
    }
}
