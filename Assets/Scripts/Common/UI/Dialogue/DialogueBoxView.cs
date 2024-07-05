using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Common.UI.Dialogue
{
    public class DialogueBoxView : AnimatableUIElement
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _sentence;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            _name.text = string.Empty;
            _sentence.text = string.Empty;
            
            await base.ActivateAsync(token);
        }

        public void UpdateName(string newName) => _name.text = newName;
        
        public void UpdateSentence(string newSentence) => _sentence.text = newSentence;
    }
}