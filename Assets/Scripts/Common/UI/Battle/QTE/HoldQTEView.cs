using System;
using System.Threading;
using Common.QTE;
using Common.QTE.Templates;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle.QTE
{
    public class HoldQTEView : ConcreteQTEView
    {
        [SerializeField] private Image _filledBackground;
        [SerializeField] private Image _background;
        [SerializeField] private Image _backgroundArea;

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
            base.SetData(qteData);

            HoldQTETemplate holdQteTemplate = qteData as HoldQTETemplate;

            float backgroundWidth = holdQteTemplate.HoldDuration / data.Duration;

            _background.fillAmount = backgroundWidth * (1 - Constants.QTEButtonHoldThreshold / 4);
            _filledBackground.fillAmount = backgroundWidth * (1 - Constants.QTEButtonHoldThreshold / 4);
        }

        protected override async UniTask SuccessAsync()
        {
            _background.fillAmount = 0;
            _filledBackground.DOColor(Color.green, Constants.DefaultUITweenTime / 2);
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultUITweenTime), cancellationToken: _tokenSource.Token);

            _filledBackground.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
        }

        protected override async UniTask FailAsync()
        {
            _background.fillAmount = 0;
            
            DOTween.Sequence()
                .Append(_filledBackground.DOColor(Color.red, Constants.DefaultUITweenTime / 2))
                .Join(_filledBackground.rectTransform.DOShakeScale(Constants.DefaultUITweenTime / 2));

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultUITweenTime), cancellationToken: _tokenSource.Token);

            _filledBackground.DOFade(0, Constants.DefaultUITweenTime / 2).From(1);
        }
        
        private async UniTask ActivateAsync()
        {
            float localTimer = data.Duration;

            _background.color = Color.white;
            _filledBackground.color = Color.green;
            _background.DOFade(1, 0).From(0);
            _filledBackground.DOFade(1, 0).From(0);

            Vector3 position = _backgroundArea.rectTransform.position;
            float halfDeltaX = _backgroundArea.rectTransform.sizeDelta.x / 2;
            float speed = _backgroundArea.rectTransform.sizeDelta.x / data.Duration;
            float distance = data.Duration * speed;

            while (localTimer > 0 && _tokenSource.Token.IsCancellationRequested == false)
            {
                float fillPosition = (localTimer * speed).ReMap(0, distance, 0, 1);
                float xPosition = Mathf.Lerp(position.x + halfDeltaX,
                    position.x - halfDeltaX, fillPosition);
                Vector3 newPosition = new Vector3(xPosition, marker.rectTransform.localPosition.y);

                marker.rectTransform.localPosition = newPosition;

                if (localTimer <= data.Duration - data.OpenDelay)
                    _background.fillAmount = fillPosition;
                
                await UniTask.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime), cancellationToken: _tokenSource.Token);
            
                localTimer -= Time.fixedDeltaTime;
            }
        }
    }
}