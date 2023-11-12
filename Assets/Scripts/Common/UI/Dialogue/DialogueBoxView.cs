using TMPro;
using UnityEngine;

namespace Common.UI.Dialogue
{
    public class DialogueBoxView : AnimatableUIElement
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _sentence;

        public override void Activate()
        {
            _name.text = string.Empty;
            _sentence.text = string.Empty;
            
            base.Activate();
        }

        public void UpdateName(string newName) => _name.text = newName;
        
        public void UpdateSentence(string newSentence) => _sentence.text = newSentence;
    }
}