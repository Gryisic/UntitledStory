using System;
using Core.Extensions;
using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Func<Type, bool> Filter { get; }
        
        public TypeFilterAttribute(Type filterType)
        {
            Filter = type =>
                type.IsAbstract == false &&
                type.IsInterface == false &&
                type.IsGenericType == false &&
                type.InheritsOrImplements(filterType);
        }
    }
}