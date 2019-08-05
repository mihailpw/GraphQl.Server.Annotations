using System.Collections.Generic;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    internal static class EnumerableExtensions
    {
        public static T[] ToArray<T>(this IEnumerable<T> enumerable, int length)
        {
            var array = new T[length];
            var i = 0;
            foreach (var value in enumerable)
            {
                array[i] = value;
                i++;
            }

            return array;
        }
    }
}