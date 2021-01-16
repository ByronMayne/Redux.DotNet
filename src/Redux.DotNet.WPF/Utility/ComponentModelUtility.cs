using System;
using System.ComponentModel;
using System.Windows;

namespace ReduxSharp.WPF.Utility
{
    /// <summary>
    /// Contains utility functions for working with <see cref="DependencyObject"/> and <see cref="DependencyProperty"/>
    /// </summary>
    internal static class ComponentModelUtility
    {
        /// <summary>
        /// Returns back the <see cref="PropertyDescriptor"/> for the given property on a dependency object.
        /// </summary>
        /// <param name="dependencyObject">The instance that the property is pointing at</param>
        /// <param name="dependencyProperty">The property you want the descriptor for</param>
        public static PropertyDescriptor GetPropertyDescriptor(DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(dependencyObject.GetType());

            for (int i = 0; i < properties.Count; i++)
            {
                if (string.Equals(properties[i].Name, dependencyProperty.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return properties[i];
                }
            }

            return null;
        }
    }
}
