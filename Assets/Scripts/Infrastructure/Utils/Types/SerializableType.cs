using System;
using UnityEngine;

namespace Infrastructure.Utils.Types
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] private string _assemblyQualifiedName = string.Empty;
        
        public Type Type { get; private set; }
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _assemblyQualifiedName = Type?.AssemblyQualifiedName ?? _assemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (TryGetType(_assemblyQualifiedName, out Type type) == false)
                return;

            Type = type;
        }

        private static bool TryGetType(string typeName, out Type type)
        {
            type = Type.GetType(typeName);

            return type != null || string.IsNullOrEmpty(typeName) == false;
        }
    }
}