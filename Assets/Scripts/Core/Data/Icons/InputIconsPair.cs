using System;
using UnityEngine;

namespace Core.Data.Icons
{
    [Serializable]
    public struct InputIconsPair
    {
        [SerializeField] private Sprite _released;
        [SerializeField] private Sprite _pressed;

        public Sprite Released => _released;
        public Sprite Pressed => _pressed;
    }
}