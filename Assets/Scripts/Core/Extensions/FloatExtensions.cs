using UnityEngine;

namespace Core.Extensions
{
    public static class FloatExtensions
    {
        public static float ReMap(this float value, float fromLow, float fromHigh, float toLow, float toHigh) => 
            toLow + (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow);

        public static float ToClamped(this float value, float min, float max) => Mathf.Clamp(value, min, max);
        
        public static float ToNearestNormal(this float value) => value >= 0f ? Mathf.Ceil(value) : Mathf.Floor(value);

        public static float Reversed(this float value) => value * -1;
    }
}