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
        public bool IsEnded { get; private set; }

        public event Action<QuickTimeEvent> Succeeded;
        public event Action<QuickTimeEvent> Failed;
        public event Action SucceededInternal;
        public event Action FailedInternal;
        public event Action InputPressed;
        public event Action InputReleased;
        
        public QuickTimeEvent(QuickTimeEventTemplate data)
        {
            Data = data;

            _conditionCheckStrategy = DefineConditionCheckStrategy();
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            IsEnded = false;
            
            _state = Enums.QTEState.Started;

            _conditionCheckStrategy.Succeeded += InvokeSuccess;
            _conditionCheckStrategy.Failed += InvokeFail;
            
            _conditionCheckStrategy.Start();

            ActiveTime = 0;

            while (ActiveTime < Data.Duration && token.IsCancellationRequested == false)
            {
                if (ActiveTime >= Data.OpenDelay && _state == Enums.QTEState.Started)
                    _state = Enums.QTEState.Opened;
                
                UniTask waitTask = UniTask.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime), cancellationToken: token);
                UniTask endTask = UniTask.WaitUntil(() => IsEnded, cancellationToken: token);
                
                await UniTask.WhenAny(waitTask, endTask); 
                
                ActiveTime += Time.fixedDeltaTime;
            }
            
            if (IsEnded == false)
                InvokeFail();
        }
        
        public void Input(Enums.QTEInput input)
        {
            _conditionCheckStrategy.Input(_state, input);
            
            InputPressed?.Invoke();
        }

        public void CancelInput()
        {
            _conditionCheckStrategy.CancelInput(_state);
            
            InputReleased?.Invoke();
        }

        private void InvokeSuccess()
        {
            _state = Enums.QTEState.Succeeded;

            Debug.Log("Suc");
            
            Succeeded?.Invoke(this);
            SucceededInternal?.Invoke();
            
            End();
        }
        
        private void InvokeFail()
        {
            _state = Enums.QTEState.Failed;
            
            Debug.Log("Fail");
            
            Failed?.Invoke(this);
            FailedInternal?.Invoke();
            
            End();
        }

        private void End()
        {
            IsEnded = true;

            _conditionCheckStrategy.Succeeded -= InvokeSuccess;
            _conditionCheckStrategy.Failed -= InvokeFail;
        }
        
        private IQTEConditionCheckStrategy DefineConditionCheckStrategy()
        {
            return Data.Type switch
            {
                Enums.QTEType.Tap => new TapQTECheckStrategy(Data),
                Enums.QTEType.Hold => new HoldQTECheckStrategy(Data),
                Enums.QTEType.MultiTap => new MultiTapQTECheckStrategy(Data),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}