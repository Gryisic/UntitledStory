using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class RenameLabelAttribute : PropertyAttribute
    {
        public string Label { get; }

        /// <summary>
        /// Rename label of a field
        /// </summary>
        /// <param name="label">New label</param>
        public RenameLabelAttribute(string label)
        {
            Label = label;
        }
    }
}