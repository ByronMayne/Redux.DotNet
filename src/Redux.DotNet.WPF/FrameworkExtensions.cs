using ReduxSharp.WPF.Controls;
using System;
using System.Windows;

namespace ReduxSharp.WPF
{
    public static class FrameworkExtensions
    {
        /// <summary>
        /// From this current element this walks the heiarchy to find the store instance
        /// </summary>
        public static IStore GetStore(this FrameworkElement instance)
        {
            FrameworkElement current = instance;

            while (current != null)
            {
                if (current is StoreProvider provider)
                {
                    return provider.Store;
                }

                current = current.Parent as FrameworkElement;
            }

            throw new Exception($"A store could not be found for Store");
        }

        /// <summary>
        /// From this current element this walks the heiarchy to find the store instance
        /// </summary>
        public static IStore<T> GetStore<T>(this FrameworkElement instance)
        {
            FrameworkElement current = instance;

            while (current != null)
            {
                if (current is StoreProvider provider)
                {
                    return provider.GetStore<T>();
                }
            }

            throw new Exception($"A store could not be found for {typeof(T).FullName}");
        }
    }
}
