using System;
using System.Collections.Generic;
using System.Threading;
using Common.Battle.Interfaces;
using Common.QTE;
using Common.UI.Battle.QTE;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Battle.States
{
    public class ActionExecutionState : BattleStateBase, IDeactivatable, IBattleStateArgsRequester, IDisposable
    {
        private readonly IStateChanger<IBattleState> _stateChanger;
        private readonly IInputService _input;
        
        private readonly QuickTimeEventExecutor _eventExecutor;
        
        private readonly QTEView _qteView;

        private CancellationTokenSource _eventTokenSource;
        
        public event Func<BattleStateArgs> RequestArgs;

        public ActionExecutionState(IStateChanger<IBattleState> stateChanger, IInputService input, UI.UI ui) : base(stateChanger)
        {
            _stateChanger = stateChanger;
            _input = input;
            
            _qteView = ui.Get<QTEView>();

            _eventExecutor = new QuickTimeEventExecutor();
        }

        public void Dispose()
        {
            _eventTokenSource?.Cancel();
            _eventTokenSource?.Dispose();
        }
        
        public override void Activate()
        {
            SubscribeToEvents();
            AttachInput();
            
            _qteView.Activate();

            _eventTokenSource = new CancellationTokenSource();
            
            ExecuteAsync().Forget();
        }

        public void Deactivate()
        {
            DeAttachInput();
            UnsubscribeToEvents();
            
            _qteView.Deactivate();
        }

        private void AttachInput()
        {
            _input.Input.Battle.AttackSelect.started += OnAttackStarted;
            _input.Input.Battle.AttackSelect.canceled += OnInputCancelled;
            _input.Input.Battle.Skill.started += OnSkillStarted;
            _input.Input.Battle.Skill.canceled += OnInputCancelled;
            
            _input.Input.Battle.Enable();
        }

        private void DeAttachInput()
        {
            _input.Input.Battle.Disable();
            
            _input.Input.Battle.AttackSelect.started -= OnAttackStarted;
            _input.Input.Battle.AttackSelect.canceled -= OnInputCancelled;
            _input.Input.Battle.Skill.started -= OnSkillStarted;
            _input.Input.Battle.Skill.canceled -= OnInputCancelled;
        }
        
        private void SubscribeToEvents()
        {
            _eventExecutor.Started += OnEventStarted;
            _eventExecutor.Ended += OnEventEnded;
        }

        private void UnsubscribeToEvents()
        {
            _eventExecutor.Started -= OnEventStarted;
            _eventExecutor.Ended -= OnEventEnded;
        }
        
        private void OnSkillStarted(InputAction.CallbackContext context) => _eventExecutor.StartInput(Enums.QTEInput.Skill);
        
        private void OnAttackStarted(InputAction.CallbackContext context) => _eventExecutor.StartInput(Enums.QTEInput.Attack);
        
        private void OnInputCancelled(InputAction.CallbackContext context) => _eventExecutor.EndInput();
        
        private void OnEventStarted(QuickTimeEvent qte)
        {
            ConcreteQTEView view = _qteView.GetViewOfTypeAndSaveToMap(qte.Data.Type, qte.GetHashCode());

            qte.SucceededInternal += view.OnSuccess;
            qte.FailedInternal += view.OnFail;
            qte.InputPressed += view.OnPress;
            qte.InputReleased += view.OnRelease;
            
            view.SetData(qte.Data);
            view.Activate();
        }
        
        private void OnEventEnded(QuickTimeEvent qte)
        {
            int hashCode = qte.GetHashCode();
            ConcreteQTEView view = _qteView.GetViewByHash(hashCode);
            
            qte.SucceededInternal -= view.OnSuccess;
            qte.FailedInternal -= view.OnFail;
            qte.InputPressed -= view.OnPress;
            qte.InputReleased -= view.OnRelease;
            
            view.Deactivate();
            _qteView.ReturnViewOfType(qte.Data.Type, hashCode);
        }

        private async UniTask ExecuteAsync()
        {
            BattleStateArgs args = RequestArgs?.Invoke();
            IReadOnlyList<QuickTimeEvent> events = args.Action.Data.QuickTimeEventSequence.GetEvents();
            
            await _eventExecutor.ExecuteAsync(events, _eventTokenSource.Token);
            
            _stateChanger.ChangeState<TurnSelectionState>();
        }
    }
}