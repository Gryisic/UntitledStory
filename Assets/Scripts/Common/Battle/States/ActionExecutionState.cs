using System;
using System.Collections.Generic;
using System.Threading;
using Common.Battle.Interfaces;
using Common.Models.Impactable.Interfaces;
using Common.QTE;
using Common.QTE.Templates;
using Common.UI.Battle.QTE;
using Core.Extensions;
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

        private BattleStateArgs _args;

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
            _args = RequestArgs?.Invoke();
            
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
        
        private void OnSkillStarted(InputAction.CallbackContext context) => _eventExecutor.StartInput(Enums.Input.B);
        
        private void OnAttackStarted(InputAction.CallbackContext context) => _eventExecutor.StartInput(Enums.Input.A);
        
        private void OnInputCancelled(InputAction.CallbackContext context) => _eventExecutor.EndInput();
        
        private void OnEventStarted(QuickTimeEvent qte)
        {
            ConcreteQTEView view = _qteView.GetViewOfTypeAndSaveToMap(qte.Data.Type, qte.GetHashCode());

            qte.SucceededInternal += view.OnSuccess;
            qte.FailedInternal += view.OnFail;
            qte.InputPressed += view.OnPress;
            qte.InputReleased += view.OnRelease;
            qte.Canceled += view.OnCancel;
            
            view.SetData(qte.Data, GetEventPosition(qte.Data));
            view.Activate();
        }
        
        private void OnEventEnded(QuickTimeEvent qte)
        {
            int hashCode = qte.GetHashCode();
            
            if (_qteView.TryGetViewByHash(hashCode, out ConcreteQTEView view) == false)
                return;
            
            qte.SucceededInternal -= view.OnSuccess;
            qte.FailedInternal -= view.OnFail;
            qte.InputPressed -= view.OnPress;
            qte.InputReleased -= view.OnRelease;
            qte.Canceled -= view.OnCancel;
            
            view.Deactivate();
            _qteView.ReturnViewOfType(qte.Data.Type, hashCode);
        }

        private Vector2 GetEventPosition(QuickTimeEventTemplate data)
        {
            switch (data.Offset)
            {
                case Enums.QTEOffset.RelativeToTarget:
                    return GetRelativePosition(data.TargetSide, data.OffsetValue);
                
                case Enums.QTEOffset.Absolute:
                    return GetAbsolutePosition(data.OffsetValue);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.Offset), data.Offset, null);
            }
        }

        private Vector2 GetRelativePosition(Enums.TargetSide side, Vector2 offset)
        {
            Vector2 sidePosition = side switch
            {
                Enums.TargetSide.SameAsUnit => _args.ActiveUnit.Transform.position,
                Enums.TargetSide.OppositeToUnit => _args.Target.Transform.position,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
            
            return sidePosition + offset;
        }
        
        private Vector2 GetAbsolutePosition(Vector2 offset)
        {
            Vector2 position = _args.CameraService.WorldToScreen(_qteView.Transform);
            
            _args.CameraService.SceneCamera.GetResolutionSteps(out float width, out float height);
            
            Vector2 resolutionOffset = new Vector2(width * offset.x, height * offset.y);
            position += resolutionOffset;

            position = _args.CameraService.ScreenToWorld(position);
            
            return position;
        }
        
        private async UniTask ExecuteAsync()
        {
            IReadOnlyList<QuickTimeEvent> events = _args.Action.Data.QuickTimeEventSequence.GetEvents();
            
            await _eventExecutor.ExecuteAsync(events, _eventTokenSource.Token);
            
            if (_args.Target is IImpactable impactable)
                _args.Action.Execute(impactable, _eventExecutor.SuccessRate);
            
            _stateChanger.ChangeState<TurnSelectionState>();
        }
    }
}