using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class FitLabelAttribute : PropertyAttribute
    {
        public float FitPercent { get; }
        
        public FitLabelAttribute(float fitPercent = 0.3f)
        {
            FitPercent = fitPercent;
        }
    }
}