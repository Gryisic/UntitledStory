using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI.Dialogue
{
    public class DialogueChoiceView : AnimatableUIElement, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _text;

        private int _index;

        public event Action<int> Choosed; 

        public void UpdateIndexAndText(int index, string text)
        {
            _index = index;
            _text.text = text;
        }

        public void OnPointerClick(PointerEventData eventData) => Choosed?.Invoke(_index);
    }
}