using TMPro;
using UnityEngine;

namespace Common.UI
{
    public class DialogueWindow : UIElement
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _sentence;
        
        public override void Activate()
        {
            Clear();
            
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void UpdateName(string name) => _name.text = name;
        
        public void UpdateSentence(string sentence) => _sentence.text = sentence;

        private void Clear()
        {
            _name.text = string.Empty;
            _sentence.text = string.Empty;
        }
    }
}