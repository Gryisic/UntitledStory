using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class BindingCallbackAttribute : PropertyAttribute
    {
        public string BindingPropertyName { get; }
        
        public BindingCallbackAttribute(string bindingPropertyName)
        {
            BindingPropertyName = bindingPropertyName;
        }
    }
}