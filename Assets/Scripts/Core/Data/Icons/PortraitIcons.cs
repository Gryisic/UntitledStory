using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Utils.Types;
using UnityEngine;

namespace Core.Data.Icons
{
    public class PortraitIcons : Icons
    {
        [SerializeField] private CharacterPortraitPair[] _portraits;

        public Sprite GetPortrait(string speaker, string emotion)
        {
            CharacterPortraitPair pair = _portraits.FirstOrDefault(p => p.Speaker == speaker);

            return pair?.GetPortrait(emotion);
        }
        
        [Serializable]
        private class CharacterPortraitPair
        {
            [SerializeField] private string _speaker;
            [SerializeField] private EmotionsMap[] _emotionsMap;

            public string Speaker => _speaker;

            public Sprite GetPortrait(string emotion)
            {
                EmotionsMap map = _emotionsMap.First(v => v.Name == emotion);
                
                return map.Portrait;
            }
            
            [Serializable]
            private struct EmotionsMap
            {
                [SerializeField] private string _name;
                [SerializeField] private Sprite _portrait;

                public string Name => _name;
                public Sprite Portrait => _portrait;
            }
        }
    }
}