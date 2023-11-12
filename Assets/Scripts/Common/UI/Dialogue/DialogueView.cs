using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Dialogue
{
    public class DialogueView : AnimatableUIElement
    {
        [SerializeField] private DialogueBoxView _boxView;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            await _boxView.ActivateAsync(token);
        }
        
        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await _boxView.DeactivateAsync(token);
            
            Deactivate();
        }

        public void UpdateName(string newName) => _boxView.UpdateName(newName);
        
        public void UpdateSentence(string newSentence) => _boxView.UpdateSentence(newSentence);
    }
}