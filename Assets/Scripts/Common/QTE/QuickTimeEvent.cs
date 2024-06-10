using System;
using System.Threading;
using Common.QTE.CheckStrategy;
using Common.QTE.EndBehaviour;
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

        private Enums.Input _lastInput;
        
        public QuickTimeEventTemplate Data { get; }
        
        public float ActiveTime { get; private set; }
        public bool IsEnded { get; private set; }

        public event Action<QuickTimeEvent> Succeeded;
        public event Action<QuickTimeEvent> Failed;
        public event Action SucceededInternal;
        public event Action FailedInternal;
        public event Action Canceled;
        public event Action<Enums.Input> InputPressed;
        public event Action<Enums.Input> InputReleased;
        public event Action<QuickTimeEvent, QTESuppressArgs> SequenceSuppressed;

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
            
            if (Data.EndBehaviour is SuppressSequenceBehaviour suppressBehaviour)
                suppressBehaviour.SequenceSuppressed += OnSequenceSuppressed;
            
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

        public void Cancel()
        {
            End();
            
            Canceled?.Invoke();
        }

        public void Input(Enums.Input input)
        {
            _lastInput = input;
            
            _conditionCheckStrategy.Input(_state, input);

            InputPressed?.Invoke(input);
        }

        public void CancelInput()
        {
            _conditionCheckStrategy.CancelInput(_state);
            
            InputReleased?.Invoke(_lastInput);
        }

        private void InvokeSuccess()
        {
            _state = Enums.QTEState.Succeeded;

            Data.EndBehaviour?.Update(_state);

            Succeeded?.Invoke(this);
            SucceededInternal?.Invoke();

            End();
        }
        
        private void InvokeFail()
        {
            _state = Enums.QTEState.Failed;
            
            Data.EndBehaviour?.Update(_state);

            Failed?.Invoke(this);
            FailedInternal?.Invoke();
            
            End();
        }

        private void End()
        {
            IsEnded = true;

            _conditionCheckStrategy.Succeeded -= InvokeSuccess;
            _conditionCheckStrategy.Failed -= InvokeFail;
            
            Data.EndBehaviour?.Clear();
        }
        
        private void OnSequenceSuppressed(QTESuppressArgs args) => SequenceSuppressed?.Invoke(this, args);

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