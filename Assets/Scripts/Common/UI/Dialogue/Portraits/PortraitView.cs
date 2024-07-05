using Common.Dialogues.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Dialogue.Portraits
{
    public class PortraitView : AnimatableUIElement
    {
        [SerializeField] private Image _portrait;
        
        public SpeakerData Data { get; private set; }

        public void UpdateData(SpeakerData data) => Data = data;

        public void UpdatePortrait(Sprite portrait) => _portrait.sprite = portrait;

        public void SetColor(Color color) => _portrait.color = color;
    }
}