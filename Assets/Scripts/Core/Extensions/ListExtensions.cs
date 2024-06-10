using System.Collections.Generic;

namespace Core.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);

            return list[index];
        }
        
        public static T Random<T>(this IReadOnlyList<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);

            return list[index];
        }

        public static bool TryAdd<T>(this List<T> list, T value)
        {
            if (list.Contains(value) == false)
            {
                list.Add(value);
                
                return true;
            }

            return false;
        }
    }
}