using System;
using UnityEngine;

namespace Core.Extensions
{
    public static class MathExtensions
    {
        public static float DecimalPart(this float value) => value - (float) Math.Truncate(value);

        public static float RoundToNearest(this float value) => Mathf.Abs(DecimalPart(value)) >= 0.5f ? Mathf.Ceil(value) : Mathf.Floor(value);
        
        public static float SnapTo(this float value, float snapTo)
        {
            float integerPart = (float)Math.Truncate(value);
            int snapDirection = integerPart > 0 ? 1 : -1;
            
            return integerPart + snapTo * snapDirection;
        }
    }
}