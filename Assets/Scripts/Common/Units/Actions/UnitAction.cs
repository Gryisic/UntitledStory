using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Units.Actions
{
    public abstract class UnitAction
    {
        private readonly Queue<Action> _callbacks;

        protected UnitAction()
        {
            _callbacks = new Queue<Action>();
        }

        public abstract void Cancel();

        public virtual async UniTask ExecuteAsync(CancellationToken token)
        {
            while (_callbacks.TryDequeue(out Action callback)) 
                callback?.Invoke();
        }

        public void AddCallback(Action callback)
        {
            if (callback == null)
                return;
        
            _callbacks.Enqueue(callback);
        }
    }
}