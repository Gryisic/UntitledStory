using System;
using System.Linq;

namespace Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool InheritsOrImplements(this Type type, Type baseType)
        {
            type = ResolveGenericType(type);
            baseType = ResolveGenericType(baseType);

            while (type != typeof(object))
            {
                if (baseType == type || HasAnyInterfaces(type, baseType))
                    return true;

                type = ResolveGenericType(type.BaseType);

                if (type == null)
                    return false;
            }

            return false;
        }
        
        private static Type ResolveGenericType(Type type)
        {
            if (type is not { IsGenericType: true })
                return type;

            Type genericType = type.GetGenericTypeDefinition();

            return genericType != type ? genericType : type;
        }

        private static bool HasAnyInterfaces(Type type, Type interfaceType) =>
            type.GetInterfaces().Any(t => ResolveGenericType(t) == interfaceType);
    }
}