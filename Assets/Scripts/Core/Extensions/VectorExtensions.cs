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
        
        public static Vector3 Normalized(this Vector3 vector, float magnitude)
        {
            if (magnitude > 9.999999747378752E-06)
                vector /= magnitude;
            else
                vector = Vector3.zero;

            return vector;
        }

        public static Vector2 SnappedTo(this Vector2 vector, float snapMarker)
        {
            float x = vector.x.SnapTo(snapMarker);
            float y = vector.y.SnapTo(snapMarker);

            return new Vector2(x, y);
        }
        
        public static Vector3 SnappedTo(this Vector3 vector, float snapMarker) => SnappedTo((Vector2) vector, snapMarker);
    }
}