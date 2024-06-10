using System;
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
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;

        private const float TweenTime = Constants.DefaultUITweenTime;
        
        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            await DOTween.Sequence()
                .Append(_name.DOFade(1, TweenTime).From(0))
                .Join(_background.DOFade(1, TweenTime).From(0))
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await DOTween.Sequence()
                .Append(_name.DOFade(0, TweenTime).From(1))
                .Join(_background.DOFade(0, TweenTime).From(1))
                .ToUniTask(cancellationToken: token);
            
            Deactivate();
        }

        public void Flip(Enums.BattleFieldSide side)
        {
            Transform iconTransform = _icon.transform;
            float absoluteIconX = Mathf.Abs(iconTransform.localPosition.x);
            
            switch (side)
            {
                case Enums.BattleFieldSide.Left:
                    iconTransform.localPosition = new Vector3(absoluteIconX, iconTransform.localPosition.y);
                    _background.transform.rotation = new Quaternion(0, 180, 0, 0);
                    break;
                
                case Enums.BattleFieldSide.Right:
                    iconTransform.localPosition = new Vector3(absoluteIconX * -1, iconTransform.localPosition.y);
                    _background.transform.rotation = new Quaternion(0, 0, 0, 0);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }
    }
}