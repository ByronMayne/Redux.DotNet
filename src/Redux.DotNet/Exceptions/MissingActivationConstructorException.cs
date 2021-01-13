using System;
using System.Collections.Generic;
using System.Linq;

namespace ReduxSharp.Exceptions
{
    public class MissingActivationConstructorException : ReduxSharpException
    {
        public Type ClassType { get; }

        public IReadOnlyList<Type> ExpectedParameters { get; }


        public MissingActivationConstructorException(Type classType, IEnumerable<Type> expectedParameters) : base(FormatMessage(classType, expectedParameters))
        {
            ClassType = classType;
            ExpectedParameters = new List<Type>(expectedParameters);
        }

        private static string FormatMessage(Type type, IEnumerable<Type> expectedParameters)
            => $"The type {type.FullName} was attempted to be activated however it was missing the required constructor. " +
                $" You must define a constructor that has the following siganture 'public {type.Name}({string.Join(", ", expectedParameters.Select(p => p.FullName))}`.";
    }
}
