using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE
{
    public class QuickTimeEventExecutor
    {
        private int _succeededEvents;
        private int _eventsCount;

        private QuickTimeEvent _currentEvent;
        
        private Enums.QTEInput _currentInput;

        private Queue<QuickTimeEvent> _eventsQueue;

        public event Action<QuickTimeEvent> Started;
        public event Action<QuickTimeEvent> Ended;

        public int SuccessRate => Mathf.CeilToInt((float) _succeededEvents / _eventsCount * 100);

        public QuickTimeEventExecutor()
        {
            _eventsQueue = new Queue<QuickTimeEvent>();
        }
        
        public async UniTask ExecuteAsync(IReadOnlyList<QuickTimeEvent> events, CancellationToken token)
        {
            foreach (var quickTimeEvent in events)
            {
                _eventsQueue.Enqueue(quickTimeEvent);
                
                await UniTask.Delay(TimeSpan.FromSeconds(quickTimeEvent.Data.StartDelay), cancellationToken: token);

                _currentEvent ??= _eventsQueue.Dequeue();
                
                SubscribeToEvents(quickTimeEvent);

                quickTimeEvent.StartAsync(token).Forget();
                
                Started?.Invoke(quickTimeEvent);
            }

            await UniTask.WaitUntil(() => _eventsQueue.Count <= 0 && _currentEvent == null, cancellationToken: token);
        }

        public void StartInput(Enums.QTEInput input)
        {
            if (_currentEvent == null)
                return;
            
            if (_currentInput != input)
                EndInput();
            
            _currentEvent.Input(input);
        }

        public void EndInput()
        {
            _currentEvent?.CancelInput();
        }

        private void SubscribeToEvents(QuickTimeEvent qte)
        {
            qte.Succeeded += OnQTESucceeded;
            qte.Failed += OnQTEFailed;
        }
        
        private void UnsubscribeToEvents(QuickTimeEvent qte)
        {
            qte.Succeeded -= OnQTESucceeded;
            qte.Failed -= OnQTEFailed;
        }
        
        private void OnQTESucceeded(QuickTimeEvent qte) => ValidateAndEndQTE(qte, _succeededEvents + 1);

        private void OnQTEFailed(QuickTimeEvent qte) => ValidateAndEndQTE(qte, _succeededEvents - 1);

        private void ValidateAndEndQTE(QuickTimeEvent qte, int valueToValidate)
        {
            _succeededEvents = Mathf.Clamp(valueToValidate, 0, _eventsCount);
            
            UnsubscribeToEvents(qte);
            
            Ended?.Invoke(_currentEvent);

            _currentEvent = _eventsQueue.TryDequeue(out QuickTimeEvent quickTimeEvent) ? quickTimeEvent : null;
        }
    }
}