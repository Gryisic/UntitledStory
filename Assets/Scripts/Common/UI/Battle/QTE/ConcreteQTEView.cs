using System;
using System.Threading;
using Common.QTE;
using Common.QTE.Templates;
using Core.Data.Icons;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle.QTE
{
    public abstract class ConcreteQTEView : UIElement, IDisposable
    {
        [SerializeField] protected Image marker;
        
        protected QuickTimeEventTemplate data;
        protected CancellationTokenSource tokenSource;

        private InputIcons _icons;

        public void Dispose()
        {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
        }
        
        protected abstract UniTask SuccessAsync();
        
        protected abstract UniTask FailAsync();
        
        public virtual void OnPress(Enums.Input input) => marker.sprite = _icons.GetPressedIcon(input);

        public virtual void OnRelease(Enums.Input input) => marker.sprite = _icons.GetReleasedIcon(input);

        public virtual void SetData(QuickTimeEventTemplate qteData, Vector2 position)
        {
            data = qteData;
            Transform.position = position;

            marker.sprite = _icons.GetReleasedIcon(data.Input);
        }

        public void SetIcons(InputIcons icons) => _icons = icons;

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

        public void OnCancel()
        {
            ResetToken();
            
            Deactivate();
        }

        private void ResetToken()
        {
            tokenSource.Cancel();
            
            tokenSource = new CancellationTokenSource();
        }
    }
}