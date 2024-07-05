using System;
using System.Threading;
using Common.UI.Dialogue.Portraits;
using Common.UI.Interfaces;
using Core.Data.Icons;
using Core.Data.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Dialogue
{
    public class DialogueView : AnimatableUIElement, IIconsRequester
    {
        [SerializeField] private DialogueBoxView _boxView;
        [SerializeField] private PortraitsView _portraitsView;
        [SerializeField] private DialogueChoicesView _choicesView;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            await _boxView.ActivateAsync(token);
            await _portraitsView.ActivateAsync(token);
        }
        
        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await _portraitsView.DeactivateAsync(token);
            await _boxView.DeactivateAsync(token);
            
            Deactivate();
        }

        public T GetView<T>() where T : UIElement
        {
            if (typeof(T) == typeof(DialogueBoxView))
                return _boxView as T;
            
            if (typeof(T) == typeof(DialogueChoicesView))
                return _choicesView as T;

            if (typeof(T) == typeof(PortraitsView))
                return _portraitsView as T;

            throw new ArgumentOutOfRangeException($"Trying to get invalid view from 'DialogueView'. Type of view {typeof(T)}");
        }
        
        public void SetIconsData(IIconsData iconsData) => 
            _portraitsView.SetIcons(iconsData.GetIcons<PortraitIcons>());
    }
}