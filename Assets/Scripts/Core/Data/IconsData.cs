using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data.Interfaces;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Core/Data/Icons")]
    public class IconsData : GameData, IIconsData
    {
        [SerializeField, Expandable] private List<Icons.Icons> _icons;

#if UNITY_EDITOR

        public static string IconsPropertyName => nameof(_icons);
        
#endif
        
        public T GetIcons<T>() where T : Icons.Icons
        {
            try
            {
                return _icons.First(i => i is T) as T;
            }
            catch
            {
                Debug.LogError($"Trying to get icons of type that is not added to icons dataset. Type: {typeof(T)}");
                throw;
            }
        }
    }
}