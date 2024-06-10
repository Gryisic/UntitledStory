using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle.QTE
{
    public class TapQTEView : ConcreteQTEView
    {
        [SerializeField] private Image _timer;

        public override void Activate()
        {
            base.Activate();

            tokenSource = new CancellationTokenSource();
            
            ActivateAsync().Forget();
        }

        public override void Deactivate()
        {
            tokenSource.Cancel();
            
            base.Deactivate();
        }

        protected override async UniTask SuccessAsync()
        {
            _timer.DOColor(Color.green, Constants.DefaultUITweenTime / 2);
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultUITweenTime), cancellationToken: tokenSource.Token);

            _timer.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
        }
        
        protected override async UniTask FailAsync()
        {
            DOTween.Sequence()
                .Append(_timer.DOColor(Color.red, Constants.DefaultUITweenTime / 2))
                .Join(_timer.rectTransform.DOShakeScale(Constants.DefaultUITweenTime / 2));

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultUITweenTime), cancellationToken: tokenSource.Token);

            _timer.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
        }
        
        private async UniTask ActivateAsync()
        {
            float localTimer = data.Duration;
            
            _timer.color = Color.white;
            _timer.DOFade(1, 0).From(0);

            while (localTimer > 0 && tokenSource.Token.IsCancellationRequested == false)
            {
                _timer.fillAmount = localTimer / data.Duration;
                
                await UniTask.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime), cancellationToken: tokenSource.Token);

                localTimer -= Time.fixedDeltaTime;
            }
        }
    }
}