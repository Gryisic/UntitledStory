using UnityEngine;

namespace Core.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 Normalized(this Vector2 vector, float magnitude)
        {
            if (magnitude > 9.999999747378752E-06)
                vector /= magnitude;
            else
                vector = Vector2.zero;

            return vector;
        }
    }
}