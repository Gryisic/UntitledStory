using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data.Interfaces;
using Infrastructure.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Data.Texts
{
    public abstract class LocalizationProvider<T> : ILocalizableTextProvider where T: class, ILocalizableTextData
    {
        [JsonProperty(PropertyName = "Russian")] protected List<T> russian;
        [JsonProperty(PropertyName = "English")] protected List<T> english;

        private Enums.Language _language;
        
        public void SetLanguage(Enums.Language language) => _language = language;
        
        public virtual ILocalizableTextData GetLocalization(string key)
        {
            switch (_language)
            {
                case Enums.Language.Russian:
                    return russian.FirstOrDefault(n => n.Key == key);
                
                case Enums.Language.English:
                    return english.FirstOrDefault(n => n.Key == key) as T;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(_language), _language, null);
            }
        }
        
        public virtual ILocalizableTextData GetLocalization(int id)
        {
            switch (_language)
            {
                case Enums.Language.Russian:
                    return TryGetDataWithID(russian, id);
                
                case Enums.Language.English:
                    return TryGetDataWithID(english, id);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(_language), _language, null);
            }
        }

        private T TryGetDataWithID(IReadOnlyList<T> sequence, int id)
        {
            try
            {
                T data = sequence.First(n => n.ID == id);

                return data;
            }
            catch 
            {
                return null;
            }
        }
    }
}