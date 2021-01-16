using Redux.DotNet.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ReduxSharp.Reflection
{
    public class PropertyPathException : Exception
    {
        public PropertyPathException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Contains helpers functions for working with 
    /// </summary>
    internal static class ReflectionUtility
    {
        private static Regex m_indexerAccess;
        private static IDictionary<Type, ConstructorInfo> s_copyConstructors;

        static ReflectionUtility()
        {
            // Must start with '[' be filled with numbers and end with ']'.
            m_indexerAccess = new Regex(@"^\[(.+)]$", RegexOptions.Compiled);
            s_copyConstructors = new Dictionary<Type, ConstructorInfo>();
        }

        /// <summary>
        /// Generates a list of the names of all the properties that have changed. 
        /// </summary>
        /// <typeparam name="T">The instance type</typeparam>
        /// <param name="lhs">The left hand value</param>
        /// <param name="rhs">The right hand value</param>
        /// <returns>The list of all the difference</returns>
        public static IList<string> GetDifferentPropertyNames<T>(T lhs, T rhs)
        {
            List<string> differences = new List<string>();
            GetDifferencet(typeof(T), lhs, rhs, null, differences);
            return differences;
        }

        /// <summary>
        /// Uses the copy constructor if one is defined otherwise throws an exception
        /// </summary>
        public static object CopyInstance(Type type, object instance)
        {
            if (s_copyConstructors.ContainsKey(type))
            {
                return s_copyConstructors[type];
            }

            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null, new Type[] { type }, Array.Empty<ParameterModifier>());

            // TODO: in the future support other methods?
            if (constructorInfo == null)
            {
                throw new MissingCopyConstructorException(type);
            }

            return constructorInfo.Invoke(new object[] { instance });
        }

        private static void GetDifferencet(Type type, object lhs, object rhs, string basePath, IList<string> results)
        {
            static string Join(string basePath, string append)
                => basePath == null ? append : $"{basePath}.{append}";

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.CanRead) continue;

                object pRhs = propertyInfo.GetValue(rhs);
                object pLhs = propertyInfo.GetValue(lhs);

                if (pRhs is IComparable comparable)
                {
                    if (comparable.CompareTo(pLhs) != 0)
                    {
                        results.Add(Join(basePath, propertyInfo.Name));
                    }
                }

                if (!IsPrimitiveOrString(propertyInfo.PropertyType))
                {
                    GetDifferencet(propertyInfo.PropertyType, pRhs, pLhs, Join(basePath, propertyInfo.Name), results);
                }
            }
        }

        public static bool IsPrimitiveOrString(this Type type)
            => type.IsPrimitive || typeof(string) == type;

        /// <summary>
        /// Takes in a path `Person.Parent.Age` and breaks it apart into sections and 
        /// walks the object tree to find the value at the end. In this example it would
        /// be the person's parents age. Thes only accesses properties
        /// </summary>
        /// <param name="instance">The instance to fetch the value from</param>
        /// <param name="path">The path to search</param>
        /// <returns>The value</returns>
        public static object GetValueFromPath(object instance, string propertyPath)
        {
            if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            string[] properties = propertyPath.Split('.');

            for (int i = 0; i < properties.Length; i++)
            {
                if (instance == null)
                {
                    return null;
                }

                Type instanceType = instance.GetType();

                string property = properties[i];

                // Array 
                Match indexer = m_indexerAccess.Match(property);
                if (indexer.Success)
                {
                    switch (instance)
                    {
                        case IList list:
                            {
                                int index = int.Parse(indexer.Value);
                                instance = list[index];
                            }
                            break;
                        case IDictionary dictionary:
                            {
                                object index = indexer.Value;
                                Type[] genericArgumetns = instanceType.GetGenericArguments();
                                instance = dictionary[Convert.ChangeType(index, genericArgumetns[0])];
                            }
                            break;
                    }

                    int arrayIndex = int.Parse(indexer.Groups[0].Value);
                    IList asList = instance as IList;

                    if (asList == null)
                    {
                        throw new PropertyPathException($"The property path {propertyPath} contains an array " +
                            $"accessor {property} however the property trying to be accessed is not of type {nameof(IList)}. The type " +
                            $"found was {instance.GetType()}");
                    }

                    instance = asList[i];
                    continue;
                }

                PropertyInfo propertyInfo = instanceType.GetProperty(property);

                if (propertyInfo == null || !propertyInfo.CanRead)
                {
                    throw new PropertyPathException($"Unable to find readable property named {property} on the type {instanceType.FullName}.");
                }

                instance = propertyInfo.GetValue(instance);
            }

            return instance;
        }

    }
}
