using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class FromParentAttribute : PropertyAttribute
    {
        public string Property { get; }
        
        public FromParentAttribute(string property)
        {
            Property = property;
        }
    }
}