using System;
using System.Text;
using Core.Configs.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data
{
    public class TextsData : ITextsData
    {
        private const string LocalizationPath = "Localization";
        
        private readonly IGameSettingsConfig _gameSettingsConfig;
        private readonly StringBuilder _stringBuilder;
        
        public TextsData(IGameSettingsConfig gameSettingsConfig)
        {
            _gameSettingsConfig = gameSettingsConfig;
            
            _stringBuilder = new StringBuilder();
        }
        
        public TextAsset GetText(string key)
        {
            _stringBuilder.Clear();
            
            string path = _stringBuilder
                .Append(LocalizationPath)
                .Append("/")
                .Append(_gameSettingsConfig.Language)
                .Append("/")
                .Append(key)
                .ToString();
            
            TextAsset text = Resources.Load<TextAsset>(path);

            if (text == null)
                throw new NullReferenceException($"Text asset at path: {path} couldn't be founded");
            
            return text;
        }
    }
}