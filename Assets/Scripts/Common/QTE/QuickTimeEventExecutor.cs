using System;
using System.Collections.Generic;
using System.Threading;
using Common.QTE.EndBehaviour;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE
{
    public class QuickTimeEventExecutor
    {
        private readonly Queue<QuickTimeEvent> _eventsQueue;
        
        private int _succeededEvents;
        private int _eventsCount;

        private bool _isSuppressed;
        
        private QuickTimeEvent _currentEvent;
        
        private Enums.Input _currentInput;
        
        public event Action<QuickTimeEvent> Started;
        public event Action<QuickTimeEvent> Ended;

        public int SuccessRate => Mathf.CeilToInt((float)_succeededEvents / _eventsCount * 100);

        public QuickTimeEventExecutor()
        {
            _eventsQueue = new Queue<QuickTimeEvent>();
        }
        
        public async UniTask ExecuteAsync(IReadOnlyList<QuickTimeEvent> events, CancellationToken token)
        {
            _isSuppressed = false;

            _eventsCount = events.Count;
            _succeededEvents = 0;
            
            foreach (var quickTimeEvent in events)
            {
                _eventsQueue.Enqueue(quickTimeEvent);
                
                await UniTask.Delay(TimeSpan.FromSeconds(quickTimeEvent.Data.StartDelay), cancellationToken: token);

                if (_isSuppressed)
                    break;
                
                _currentEvent ??= _eventsQueue.Dequeue();
                
                SubscribeToEvents(quickTimeEvent);

                quickTimeEvent.StartAsync(token).Forget();
                
                Started?.Invoke(quickTimeEvent);
            }

            await UniTask.WaitUntil(() => _eventsQueue.Count <= 0 && _currentEvent == null, cancellationToken: token);
        }

        public void StartInput(Enums.Input input)
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
            qte.SequenceSuppressed += OnSequenceSuppressed;
        }

        private void UnsubscribeToEvents(QuickTimeEvent qte)
        {
            qte.Succeeded -= OnQTESucceeded;
            qte.Failed -= OnQTEFailed;
            qte.SequenceSuppressed -= OnSequenceSuppressed;
        }
        
        private void OnQTESucceeded(QuickTimeEvent qte) => ValidateAndEndQTE(qte, _succeededEvents + 1);

        private void OnQTEFailed(QuickTimeEvent qte) => ValidateAndEndQTE(qte, _succeededEvents);

        private void ValidateAndEndQTE(QuickTimeEvent qte, int valueToValidate)
        {
            _succeededEvents = Mathf.Clamp(valueToValidate, 0, _eventsCount);
            
            UnsubscribeToEvents(qte);
            
            Ended?.Invoke(_currentEvent);

            _currentEvent = _eventsQueue.TryDequeue(out QuickTimeEvent quickTimeEvent) ? quickTimeEvent : null;
        }
        
        private void OnSequenceSuppressed(QuickTimeEvent qte, QTESuppressArgs args)
        {
            _isSuppressed = true;

            int finalData = GetUpdatedData(args.DataUpdate);
            
            while (_currentEvent != null)
            {
                _currentEvent.Cancel();
                
                ValidateAndEndQTE(_currentEvent, finalData);
            }

            _currentEvent = null;
        }

        private int GetUpdatedData(Enums.QTEDataUpdate dataUpdate)
        {
            return dataUpdate switch
            {
                Enums.QTEDataUpdate.Preserve => _succeededEvents,
                Enums.QTEDataUpdate.ToHundred => _eventsCount,
                Enums.QTEDataUpdate.ToZero => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(dataUpdate), dataUpdate, null)
            };
        }
    }
}