using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Dialogue
{
    public class DialogueView : AnimatableUIElement
    {
        [SerializeField] private DialogueBoxView _boxView;
        [SerializeField] private DialogueChoicesView _choicesView;

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

        public T GetView<T>() where T : UIElement
        {
            if (typeof(T) == typeof(DialogueBoxView))
                return _boxView as T;
            
            if (typeof(T) == typeof(DialogueChoicesView))
                return _choicesView as T;

            throw new ArgumentOutOfRangeException($"Trying to get invalid view from 'DialogueView'. Type of view {typeof(T)}");
        }

        //public void UpdateName(string newName) => _boxView.UpdateName(newName);
        
        //public void UpdateSentence(string newSentence) => _boxView.UpdateSentence(newSentence);
    }
}