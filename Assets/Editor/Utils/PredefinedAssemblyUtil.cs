using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Editor.Utils
{
    public static class PredefinedAssemblyUtil
    {
        private enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass,
        }

        public static IReadOnlyList<Type> GetTypes(Type interfaceType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            List<Type> types = new();
            
            for (var i = 0; i < assemblies.Length; i++)
            {
                AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
                
                if (assemblyType != null)
                    assemblyTypes.Add((AssemblyType) assemblyType, assemblies[i].GetTypes());
            }
            
            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharp], types, interfaceType);
            AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharpFirstPass], types, interfaceType);

            return types;
        }

        public static Type GetTypeByName(Type interfaceType, string name)
        {
            IReadOnlyList<Type> types = GetTypes(interfaceType);

            return types.First(t => t.Name == name);
        }
        
        private static AssemblyType? GetAssemblyType(string type)
        {
            return type switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null
            };
        }

        private static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
        {
            if (assembly == null)
                return;

            foreach (var type in assembly)
            {
                if (type != interfaceType && interfaceType.IsAssignableFrom(type))
                    types.Add(type);
            }
        }
    }
}