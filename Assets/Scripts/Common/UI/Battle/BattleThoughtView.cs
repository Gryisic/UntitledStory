using TMPro;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleThoughtView : AnimatableUIElement
    {
        [SerializeField] private TextMeshProUGUI _thoughts;
        
        private const float StartPosition = 850;
        private const float EndPosition = -1350;
        
        public int Size => _thoughts.text.Length;

        public void Add(string thought)
        {
            _thoughts.text = $"{_thoughts.text}{thought}";
        }

        public void Clear() => _thoughts.text = string.Empty;
    }
}