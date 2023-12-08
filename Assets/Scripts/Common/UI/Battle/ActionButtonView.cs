using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle
{
    public class ActionButtonView : AnimatableUIElement
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _background;

        private const float TweenTime = Constants.DefaultUITweenTime;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            await DOTween.Sequence()
                .Append(_name.DOFade(1, TweenTime).From(0))
                .Join(_background.DOFade(0.25f, TweenTime).From(0))
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await DOTween.Sequence()
                .Append(_name.DOFade(0, TweenTime).From(1))
                .Join(_background.DOFade(0, TweenTime).From(0.25f))
                .ToUniTask(cancellationToken: token);
            
            Deactivate();
        }
    }
}