using System;
using System.Collections.Generic;
using System.Text;

namespace ReduxSharp
{
    internal static class EnumerableUtility
    {
        public static IEnumerable<T> ToEnumerable<T>(T item)
        {
            if (item != null)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> ToNullableEnumerable<T>(T item)
        {
            yield return item;
        }
    }

    internal static class EnumerableExtensions
    {
        public static IList<T> Clone<T>(this IList<T> instance, params T[] addtionalItems)
        {
            int startingCount = instance.Count;
            int count = instance.Count + addtionalItems.Length;

            T[] clone = new T[count];

            for(int i = 0; i < count; i++)
            {
                if(i < startingCount)
                {
                    clone[i] = instance[i];
                }
                else
                {
                    clone[i] = addtionalItems[i - startingCount];
                }
            }

            return clone;
        }

        /// <summary>
        /// Returns back each element that is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IEnumerable<T> IsNotNull<T>(this IEnumerable<T> instance) where T : class
        {
            foreach (T element in instance)
            {
                if (instance != null)
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Removes all elements from a list that match a condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int RemoveWhere<T>(this IList<T> instance, Predicate<T> condition, int max = -1)
        {
            int removedCount = 0;
            for (int i = instance.Count - 1; i >= 0; i--)
            {
                if (!condition(instance[i]))
                {
                    if (max == 0)
                    {
                        return removedCount;
                    }

                    instance.RemoveAt(i);
                    removedCount++;

                    max--;
                }
            }
            return removedCount;
        }
    }
}
