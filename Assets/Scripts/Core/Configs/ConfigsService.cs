using System;
using Core.Configs.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "Core/Configs/ConfigsService")]
    public class ConfigsService : ScriptableObject, IConfigsService
    {
        [SerializeField] private Config[] _configs;
        
        public T GetConfig<T>() where T : IConfig
        {
            foreach (var config in _configs)
            {
                if (config is T casted)
                    return casted;
            }

            throw new NullReferenceException($"Configs service doesn't contain {typeof(T)} config");
        }
    }
}