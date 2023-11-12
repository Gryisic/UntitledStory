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
    }
}