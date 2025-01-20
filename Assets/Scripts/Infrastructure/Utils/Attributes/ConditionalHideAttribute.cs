using System;
using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string SourceField { get; }

        /// <summary>
        /// Hides a field if 'sourceField' bool is false.
        /// </summary>
        /// <param name="sourceField">Bool value used to determine visibility of a field</param>
        public ConditionalHideAttribute(string sourceField)
        {
            SourceField = sourceField;
        }
    }
}