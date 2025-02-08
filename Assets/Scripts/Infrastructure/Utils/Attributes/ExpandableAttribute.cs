using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class ExpandableAttribute : PropertyAttribute
    {
        public bool DisableManualObjectAssignment { get; }
        
        public ExpandableAttribute(bool disableManualObjectAssignment = false)
        {
            DisableManualObjectAssignment = disableManualObjectAssignment;
        }
    }
}