using System;
using System.IO;
using System.Text;
using Core.Configs.Interfaces;
using Core.Data.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Data
{
    public class TextsData : ITextsData
    {
        private const string LocalizationPath = "Localization";
        private const string JsonDataPath = "Json";
        
        private readonly char _separator = Path.AltDirectorySeparatorChar;
        
        private readonly IGameSettingsConfig _gameSettingsConfig;
        private readonly StringBuilder _stringBuilder;

        private ILocalizableTextProvider _lastProvider;
        
        public TextsData(IGameSettingsConfig gameSettingsConfig)
        {
            _gameSettingsConfig = gameSettingsConfig;
            
            _stringBuilder = new StringBuilder();
        }
        
        public TextAsset GetTextAsset(string key)
        {
            _stringBuilder.Clear();
            
            string path = _stringBuilder
                .Append(LocalizationPath)
                .Append(_separator)
                .Append(_gameSettingsConfig.Language)
                .Append(_separator)
                .Append(key)
                .ToString();
            
            TextAsset text = Resources.Load<TextAsset>(path);

            if (text == null)
                throw new NullReferenceException($"Text asset at path: {path} couldn't be founded");
            
            return text;
        }
        
        public T GetLocalizedData<T>() where T: class, ILocalizableTextProvider
        {
            if (_lastProvider != null && typeof(T) == _lastProvider.GetType())
            {
                _lastProvider.SetLanguage(_gameSettingsConfig.Language);
                
                return _lastProvider as T;
            }
            
            _stringBuilder.Clear();
            
            string key = typeof(T).Name;
            
            string path = _stringBuilder.Append(LocalizationPath)
                .Append(_separator)
                .Append(JsonDataPath)
                .Append(_separator)
                .Append(key)
                .ToString();
            
            TextAsset asset = Resources.Load<TextAsset>(path);
            
            if (asset == null)
                throw new NullReferenceException($"Text asset at path: {path} couldn't be founded");

            T data = JsonConvert.DeserializeObject<T>(asset.text);
            
            data.SetLanguage(_gameSettingsConfig.Language);

            _lastProvider = data;
            
            return data;
        }
    }
}