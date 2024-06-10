using System;
using Infrastructure.Utils;
using Infrastructure.Utils.Types;
using UnityEngine;

namespace Core.Data.Icons
{
    [Serializable]
    public class InputIconsMap<T> : IconsMap where T: struct, Enum
    {
        [SerializeField] private SerializableDictionary<T, InputIconsPair> _map;

        public override Sprite GetIcon(string iconName)
        {
            bool hasPressed = iconName.Contains(Constants.PressedInputButtonDeclaration);
            string buttonDeclaration = hasPressed
                ? Constants.PressedInputButtonDeclaration
                : Constants.ReleasedInputButtonDeclaration;
            
            iconName = iconName.Replace(buttonDeclaration, string.Empty);

            _map.TryGetValue(GetEnumValue(iconName), out InputIconsPair spritesPair);
            
            return hasPressed ? spritesPair.Pressed : spritesPair.Released;
        }

        private T GetEnumValue(string valueName)
        {
            if (Enum.TryParse(valueName, out T value))
                return value;

            throw new NullReferenceException($"Icons map does not contain binding for {valueName}");
        }
    }
}