using System;
using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ExposedRangeAttribute : PropertyAttribute
    {
        public float Min { get; }
        public float Max { get; }
        public bool ShowHelper { get; }

        /// <summary>
        /// Same as range except that borders shown in inspector and value can be written as common float.
        /// </summary>
        /// <param name="min">Minimal border. By default value is '-999'.</param>
        /// <param name="max">Maximum border. By default value is '999'.</param>
        /// <param name="showHelper">Add min and max to label? By default is true.</param>
        public ExposedRangeAttribute(float min = -999, float max = 999, bool showHelper = true)
        {
            Min = min;
            Max = max;
            ShowHelper = showHelper;
        }
    }
}