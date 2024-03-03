using System;
using System.Threading;
using Common.QTE;
using Common.QTE.Templates;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle.QTE
{
    public class MultiTapQTEView : ConcreteQTEView
    {
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private Image _timer;

        private int _counterValue;

        public override void Activate()
        {
            base.Activate();

            _tokenSource = new CancellationTokenSource();
            
            ActivateAsync().Forget();
        }

        public override void Deactivate()
        {
            _tokenSource.Cancel();
            
            base.Deactivate();
        }

        public override void SetData(QuickTimeEventTemplate qteData)
        {
            if (qteData is MultiTapQTETemplate multiTapQteTemplate == false)
                throw new InvalidOperationException($"Trying to set MultiTap view via non Multi Tap template");
            
            base.SetData(qteData);

            _counterValue = multiTapQteTemplate.TapCount;
            _counter.text = $"x{_counterValue}";
        }

        public override void OnPress()
        {
            base.OnPress();

            _counterValue = Mathf.Clamp(_counterValue--, 0, _counterValue);
            
            _counter.text = $"x{_counterValue}";
        }

        protected override async UniTask SuccessAsync()
        {
            _timer.DOColor(Color.green, Constants.DefaultUITweenTime / 2);
            _counter.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultUITweenTime), cancellationToken: _tokenSource.Token);

            _timer.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
        }

        protected override async UniTask FailAsync()
        {
            DOTween.Sequence()
                .Append(_timer.DOColor(Color.red, Constants.DefaultUITweenTime / 2))
                .Join(_timer.rectTransform.DOShakeScale(Constants.DefaultUITweenTime / 2));

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultUITweenTime), cancellationToken: _tokenSource.Token);

            _timer.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
        }
        
        private async UniTask ActivateAsync()
        {
            float localTimer = data.Duration;
            
            _timer.color = Color.white;
            _timer.DOFade(1, 0).From(0);
            _counter.DOFade(1, 0).From(0);

            while (localTimer > 0 && _tokenSource.Token.IsCancellationRequested == false)
            {
                _timer.fillAmount = localTimer / data.Duration;
                
                await UniTask.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime), cancellationToken: _tokenSource.Token);

                localTimer -= Time.fixedDeltaTime;
            }
        }
    }
}