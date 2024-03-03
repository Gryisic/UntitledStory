using System;
using System.Threading;
using Common.QTE;
using Common.QTE.Templates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle.QTE
{
    public abstract class ConcreteQTEView : UIElement, IDisposable
    {
        [SerializeField] protected Image marker;
        
        protected QuickTimeEventTemplate data;
        protected CancellationTokenSource _tokenSource;

        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
        
        protected abstract UniTask SuccessAsync();
        
        protected abstract UniTask FailAsync();
        
        public virtual void OnPress() => marker.sprite = data.Pressed;

        public virtual void OnRelease() => marker.sprite = data.Released;
        
        public virtual void SetData(QuickTimeEventTemplate qteData)
        {
            data = qteData;

            marker.sprite = data.Released;
        }

        public void OnSuccess()
        {
            ResetToken();
            
            SuccessAsync().Forget();
        }

        public void OnFail()
        {
            ResetToken();
            
            FailAsync().Forget();
        }

        private void ResetToken()
        {
            _tokenSource.Cancel();
            
            _tokenSource = new CancellationTokenSource();
        }
    }
}