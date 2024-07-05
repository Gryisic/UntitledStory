using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Models.ParticlesMessage
{
    [Serializable]
    public struct SymbolsTextureData
    {
        [SerializeField] private Texture _texture;
        [SerializeField] private char[] _chars;

        private Dictionary<char, Vector2> _charPositionMap;

        public void Initialize()
        {
            _charPositionMap = new Dictionary<char, Vector2>();

            for (var i = 0; i < _chars.Length; i++)
            {
                char c = char.ToLowerInvariant(_chars[i]);
                
                if (_charPositionMap.ContainsKey(c))
                    continue;

                Vector2 uv = new Vector2(i % 10, 9 - i /10);
                
                _charPositionMap.Add(c, uv);
            }
        }

        public Vector2 GetTextureCoordinates(char c)
        {
            c = char.ToLowerInvariant(c);
            
            if (_charPositionMap == null)
                Initialize();

            return _charPositionMap.TryGetValue(c, out Vector2 coordinate) ? coordinate : Vector2.zero;
        }
    }
}