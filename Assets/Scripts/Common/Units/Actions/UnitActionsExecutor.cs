using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units.Actions
{
    public class UnitActionsExecutor : IDisposable
    {
        private readonly Queue<UnitAction> _actionsQueue;
        
        private UnitAction _activeAction;
        private CancellationTokenSource _executionTokenSource;

        private bool _isExecutionSuppressed;

        public UnitActionsExecutor()
        {
            _actionsQueue = new Queue<UnitAction>();
        }
        
        public void Dispose()
        {
            _executionTokenSource?.Cancel();
            _executionTokenSource?.Dispose();
        }

        public void SuppressActionExecution() => _isExecutionSuppressed = true;

        public void UnSuppressActionExecution() => _isExecutionSuppressed = false;

        public UnitAction AddActionToQueue(UnitAction action)
        {
            if (action == null)
                throw new NullReferenceException("Trying to add null 'Unit Action' to queue");

            _actionsQueue.Enqueue(action);

            return action;
        }

        public void CancelAllActions()
        {
            _activeAction?.Cancel();
            
            while(_actionsQueue.TryDequeue(out UnitAction action))
                action.Cancel();
            
            _executionTokenSource?.Cancel();
            _actionsQueue.Clear();

            _activeAction = null;
        }

        public void Execute() => ExecuteWithAwaiter().Forget();

        public async UniTask ExecuteWithAwaiter()
        {
            if (_actionsQueue.Count <= 0)
                throw new InvalidOperationException("Trying to execute 'Actions queue' with 0 actions in it");
            
            _executionTokenSource = new CancellationTokenSource();
            
            await ExecuteAsync(_actionsQueue.Dequeue());
        }

        private async UniTask ExecuteAsync(UnitAction action)
        {
            while (_executionTokenSource.IsCancellationRequested == false)
            {
                if (_isExecutionSuppressed)
                    await UniTask.WaitWhile(() => _isExecutionSuppressed, cancellationToken: _executionTokenSource.Token);
                
                _activeAction = action;
                
                await action.ExecuteAsync(_executionTokenSource.Token);

                if (_actionsQueue.TryDequeue(out action)) 
                    continue;
                
                break;
            }

            _activeAction = null;
        }
    }
}