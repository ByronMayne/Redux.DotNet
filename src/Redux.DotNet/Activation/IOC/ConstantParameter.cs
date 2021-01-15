using System;
using System.Diagnostics;

namespace ReduxSharp.Activation.IOC
{
    [DebuggerDisplay("{Type}: {Name}")]
    public class ConstantParameter : IParameter
    {
        public string Name { get; init; }

        public Type Type { get; init; }

        public object Value { get; init; }


        public ConstantParameter(string name, object value, Type bindAs)
        {
            Name = name;
            Value = value;
            Type = bindAs;
        }

        public static ConstantParameter Create<T>(string name, object value)
            => new ConstantParameter(name, value, typeof(T));

        bool IEquatable<IParameter>.Equals(IParameter other)
            => string.Equals(other.Name, Name) && other.Type == Type;

        object IParameter.GetValue()
            => Value;
    }
}
