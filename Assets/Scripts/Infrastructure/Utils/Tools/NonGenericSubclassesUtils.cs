#if UNITY_EDITOR
using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using Assembly = System.Reflection.Assembly;

namespace Infrastructure.Utils.Tools
{
    public static class NonGenericSubclassesUtils
    {
        private const string NameSpace = "Infrastructure.Utils.Tools.Generated";
        
        public static Type GetType(Type predefinedInstance)
        {
            string typeName = GetName(predefinedInstance.ToString());
            string fileName = $"Assets/Scripts/Infrastructure/Utils/Tools/Generated/{typeName}.cs";
            string fullName = $"{NameSpace}.{typeName}";

            if (File.Exists(fileName))
                return Type.GetType(fullName);

            if (EditorUtility.DisplayDialog("Message!", 
                    "Callback for this isn't created. Do you want to create it? (You'll need to re-select callback after creation!)",
                    "Yes!", "No!") == false)
                return null;
            
            CodeCompileUnit targetUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace(NameSpace);
            CodeAttributeDeclaration serializableDeclaration = new CodeAttributeDeclaration("System.SerializableAttribute");
            
            CodeTypeDeclaration declaration = new CodeTypeDeclaration(typeName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public,
            };
            CodeTypeReference reference = new CodeTypeReference
            {
                BaseType = predefinedInstance.ToString(),
                Options = CodeTypeReferenceOptions.GenericTypeParameter
            };

            targetUnit.Namespaces.Add(codeNamespace);
            declaration.BaseTypes.Add(reference);
            declaration.CustomAttributes.Add(serializableDeclaration);
            codeNamespace.Types.Add(declaration);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            using StreamWriter writer = new StreamWriter(fileName);
            provider.GenerateCodeFromCompileUnit(targetUnit, writer, options);
            CompilationPipeline.RequestScriptCompilation();
            AssetDatabase.Refresh();

            return Type.GetType(fullName);
        }

        public static bool HasNonGenericSubclass(Type genericType, Type concreteType)
        {
            if (genericType.IsGenericType == false)
                throw new ArgumentException($"Type {genericType} used as 'generic type' argument is non generic.");
            
            if (concreteType.IsGenericType)
                throw new ArgumentException($"Type {concreteType} used as 'concrete type' argument is generic.");

            List<Type> types = Assembly.GetAssembly(genericType)
                .GetTypes()
                .Where(t => t.IsAbstract == false &&
                            t.IsGenericType == false &&
                            concreteType.IsAssignableFrom(genericType))
                .ToList();
            
            return types.Count > 0;
        }

        private static string GetName(string typeString)
        {
            string[] splittedType = typeString.Split('[');
            
            return (from part in splittedType
                select part.Split('.').Last()
                into finalPart
                select new string(finalPart.Where(char.IsLetter).ToArray())
                into finalPart
                select finalPart.Trim()).Aggregate("",
                (current, finalPart) => $"{current}{finalPart}");
        }
    }
}
#endif