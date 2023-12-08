using System;
using System.Threading;
using Common.QTE.CheckStrategy;
using Common.QTE.Interfaces;
using Common.QTE.Templates;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE
{
    public class QuickTimeEvent
    {
        private readonly IQTEConditionCheckStrategy _conditionCheckStrategy;
        
        private Enums.QTEState _state = Enums.QTEState.Sleep;

        public QuickTimeEventTemplate Data { get; }
        
        public float ActiveTime { get; private set; }

        public event Action<QuickTimeEvent> Succeeded;
        public event Action<QuickTimeEvent> Failed;
        
        public QuickTimeEvent(QuickTimeEventTemplate data)
        {
            Data = data;

            _conditionCheckStrategy = DefineConditionCheckStrategy();
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Data.StartDelay), cancellationToken: token);
            
            _state = Enums.QTEState.Started;

            _conditionCheckStrategy.Succeeded += InvokeSuccess;
            _conditionCheckStrategy.Failed += InvokeFail;
            
            _conditionCheckStrategy.Start();

            ActiveTime = 0;

            while (ActiveTime < Data.Duration && token.IsCancellationRequested == false)
            {
                if (ActiveTime >= Data.OpenDelay && _state == Enums.QTEState.Started)
                    _state = Enums.QTEState.Opened;
                
                ActiveTime += Time.fixedDeltaTime;
            }
            
            _conditionCheckStrategy.Succeeded -= InvokeSuccess;
            _conditionCheckStrategy.Failed -= InvokeFail;
        }
        
        public void Input(Enums.QTEInput input) => _conditionCheckStrategy.Input(_state, input);

        public void CancelInput() => _conditionCheckStrategy.CancelInput(_state);

        private void InvokeSuccess()
        {
            _state = Enums.QTEState.Succeeded;
            Succeeded?.Invoke(this);
        }
        
        private void InvokeFail()
        {
            _state = Enums.QTEState.Failed;
            Failed?.Invoke(this);
        }
        
        private IQTEConditionCheckStrategy DefineConditionCheckStrategy()
        {
            return Data.Type switch
            {
                Enums.QTEType.Tap => new TapQTECheckStrategy(Data),
                Enums.QTEType.Hold => new HoldQTECheckStrategy(Data),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}