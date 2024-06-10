using System;
using System.Threading;
using Common.UI.Interfaces;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle
{
    public class TargetSelectorView : AnimatableUIElement, IQueueableUIElement
    {
        [SerializeField] private Image _selector;

        public event Action<UIElement> RequestAddingToQueue;
        
        public override async UniTask ActivateAsync(CancellationToken token)
        {
            gameObject.SetActive(true);
            
            await _selector.DOFade(1f, Constants.DefaultUITweenTime).From(0f).ToUniTask(cancellationToken: token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await _selector.DOFade(0f, Constants.DefaultUITweenTime).From(1f).ToUniTask(cancellationToken: token);
            
            gameObject.SetActive(false);
        }

        public void Activate(Enums.TargetSelectionType selectionType)
        {
            Activate();
            
            if (selectionType == Enums.TargetSelectionType.Standalone)
                RequestAddingToQueue?.Invoke(this);
        }

        public void FocusAt(IBattleUnitSharedData point) => transform.position = point.Transform.position + new Vector3(0f, -0.35f, 0f);
    }
}