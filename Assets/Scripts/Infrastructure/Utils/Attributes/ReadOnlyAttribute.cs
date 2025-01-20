using System;
using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        
    }
}