using System.Collections.Generic;
using System.Linq;
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

        public int SuccessRate => Mathf.CeilToInt((float) _succeededEvents / _eventsCount * 100);
        
        public async UniTask ExecuteAsync(IReadOnlyList<QuickTimeEvent> events, CancellationToken token)
        {
            List<UniTask> eventTasks = Enumerable.Select(events, quickTimeEvent => quickTimeEvent.StartAsync(token)).ToList();

            _succeededEvents = 0;
            _eventsCount = events.Count;

            for (var i = 0; i < events.Count; i++)
            {
                _currentEvent = events[i];
                
                SubscribeToEvents(_currentEvent);

                await UniTask.WaitUntil(() => _currentEvent.ActiveTime >= _currentEvent.Data.Duration, cancellationToken: token);
            }

            await UniTask.WhenAll(eventTasks);

            _currentEvent = null;
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
            _currentEvent.CancelInput();
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
        }
    }
}