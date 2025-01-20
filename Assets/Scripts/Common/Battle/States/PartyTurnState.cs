using System;
using System.Collections.Generic;
using System.Threading;
using Common.Battle.Interfaces;
using Common.Battle.TargetSelection;
using Common.Models.Skills;
using Common.Models.Skills.Interfaces;
using Common.UI.Battle;
using Common.UI.Interfaces;
using Common.Units.Handlers;
using Core.Data.Texts;
using Core.Extensions;
using Core.GameStates;
using Core.Interfaces;
using Core.Utils.ObservableVariables;
using Core.Utils.ObservableVariables.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Battle.States
{
    public class PartyTurnState : BattleStateBase, IDeactivatable, IResettable, IDisposable, IBattleStateArgsRequester, IBattleData
    {
        private readonly IInputService _inputService;

        private readonly TargetSelector _targetSelector;
        private readonly BattleUnitsHandler _battleUnitsHandler;
        private readonly BattleActionsView _worldUI;
        private readonly BattleOverlayView _overlayUI;

        private PartyMembersLocalization _partyMembersLocalization;

        private BattleStateArgs _args;
        
        private CancellationTokenSource _uiAnimationTokenSource;

        private bool _isActive;
        
        public event Func<BattleStateArgs> RequestArgs;

        public IObservableVariable<int> CurrentTurn { get; } = new ObservableInt();
        
        private IBattleData GetData() => this;

        public PartyTurnState(IStateChanger<IBattleState> stateChanger, IInputService inputService, BattleUnitsHandler battleUnitsHandler, UI.UI ui) : base(stateChanger)
        {
            _inputService = inputService;
            _battleUnitsHandler = battleUnitsHandler;
            _worldUI = ui.Get<BattleActionsView>();
            _overlayUI = ui.Get<BattleOverlayView>();
            _targetSelector = new TargetSelector();
        }
        
        public void Dispose()
        {
            _uiAnimationTokenSource?.Cancel();
            _uiAnimationTokenSource?.Dispose();
        }

        public override void Activate() => ActivateAsync().Forget();

        public void Deactivate()
        {
            if (_isActive == false)
                return;
            
            DeactivateAsync().Forget();
        }
        
        public void Reset()
        {
            _targetSelector.Reset();
            
            _args = null;
            _partyMembersLocalization = null;
        }
        
        private async UniTask ActivateAsync()
        {
            _isActive = true;

            _args ??= RequestArgs?.Invoke();
            _partyMembersLocalization ??= _args.TextsData.GetLocalizedData<PartyMembersLocalization>();
            
            PartyMemberLocalization memberLocalization = _partyMembersLocalization
                .GetLocalization(_battleUnitsHandler.ActiveUnit.ID).As<PartyMemberLocalization>();

            _args.ThoughtsBuilder.AppendNeutralOfUnit(_battleUnitsHandler.ActiveUnit.ID);
            
            _overlayUI.ThoughtsView.Clear();
            _overlayUI.ThoughtsView.Append(_args.ThoughtsBuilder.Build());
            _args.ThoughtsBuilder.Clear();
            
            SubscribeToEvents();

            _battleUnitsHandler.ActiveUnit.SetActive();
            
            _uiAnimationTokenSource = new CancellationTokenSource();

            Vector2 activeUnitPosition = _battleUnitsHandler.ActiveUnit.Transform.position;
            _worldUI.ValidatePosition(activeUnitPosition, _args.NavigationArea.GetPositionSide(activeUnitPosition), _args.CameraService.SceneCamera);
            _worldUI.UpdateActionPhrase(memberLocalization);
            
            await _worldUI.ActivateAsync(_uiAnimationTokenSource.Token);
            
            AttachInput();
        }
        
        private async UniTask DeactivateAsync()
        {
            _isActive = false;
            
            DeAttachInput();
            
            _targetSelector.Deactivate();
            _battleUnitsHandler.ActiveUnit.SetPassive();
            
            await _worldUI.DeactivateAsync(_uiAnimationTokenSource.Token);

            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _worldUI.RequestItemsData += GetItemsData;
            _worldUI.ActionSelected += OnActionSelected;
            _worldUI.RequestTargetSelection += _targetSelector.Activate;
            _worldUI.SuppressTargetSelection += _targetSelector.Deactivate;

            _targetSelector.Activated += _worldUI.TargetSelector.Activate;
            _targetSelector.Deactivated += _worldUI.TargetSelector.Deactivate;
            _targetSelector.TargetUpdated += _worldUI.TargetSelector.FocusAt;
            _targetSelector.TargetUpdated += _args.UpdateTargetData;
            _targetSelector.RequestTargets += _battleUnitsHandler.GetUnitsOfType;
            _targetSelector.RequestActiveUniteType += _battleUnitsHandler.ActiveUnit.GetType;
        }
        
        private void UnsubscribeToEvents()
        {
            _worldUI.RequestItemsData -= GetItemsData;
            _worldUI.ActionSelected -= OnActionSelected;
            _worldUI.RequestTargetSelection -= _targetSelector.Activate;
            _worldUI.SuppressTargetSelection -= _targetSelector.Deactivate;
            
            _targetSelector.Activated -= _worldUI.TargetSelector.Activate;
            _targetSelector.Deactivated -= _worldUI.TargetSelector.Deactivate;
            _targetSelector.TargetUpdated -= _worldUI.TargetSelector.FocusAt;
            _targetSelector.TargetUpdated -= _args.UpdateTargetData;
            _targetSelector.RequestTargets -= _battleUnitsHandler.GetUnitsOfType;
            _targetSelector.RequestActiveUniteType -= _battleUnitsHandler.ActiveUnit.GetType;
        }
        
        private void AttachInput()
        {
            _inputService.Input.Battle.AttackSelect.performed += OnSelectPerformed;
            _inputService.Input.Battle.Skill.performed += OnSkillPerformed;
            _inputService.Input.Battle.GuardCancel.performed += OnGuardPerformed;
            _inputService.Input.Battle.Items.performed += OnItemsPerformed;
            _inputService.Input.Battle.Up.performed += OnUpPerformed;
            _inputService.Input.Battle.Down.performed += OnDownPerformed;
            _inputService.Input.Battle.Left.performed += OnLeftPerformed;
            _inputService.Input.Battle.Right.performed += OnRightPerformed;

            _inputService.Input.Battle.Enable();

#if UNITY_EDITOR
            _inputService.Input.Debug.EndBattle.performed += OnBattleEnd;
            _inputService.Input.Debug.DamageHero.performed += OnDebugDamage;
            
            _inputService.Input.Debug.Enable();
#endif
        }

        private void DeAttachInput()
        {
#if UNITY_EDITOR
            _inputService.Input.Debug.EndBattle.performed -= OnBattleEnd;
            _inputService.Input.Debug.DamageHero.performed -= OnDebugDamage;
            
            _inputService.Input.Debug.Disable();
#endif
            
            _inputService.Input.Battle.Disable();
            
            _inputService.Input.Battle.AttackSelect.performed -= OnSelectPerformed;
            _inputService.Input.Battle.Skill.performed -= OnSkillPerformed;
            _inputService.Input.Battle.GuardCancel.performed -= OnGuardPerformed;
            _inputService.Input.Battle.Items.performed -= OnItemsPerformed;
            _inputService.Input.Battle.Up.performed -= OnUpPerformed;
            _inputService.Input.Battle.Down.performed -= OnDownPerformed;
            _inputService.Input.Battle.Left.performed -= OnLeftPerformed;
            _inputService.Input.Battle.Right.performed -= OnRightPerformed;
        }

#if UNITY_EDITOR
        private void OnBattleEnd(InputAction.CallbackContext context) => ToNextState<BattleFinalizeState>().Forget();

        private void OnDebugDamage(InputAction.CallbackContext context) =>
            _battleUnitsHandler.ActiveUnit.ApplyDamage(100);
#endif

        private void OnUpPerformed(InputAction.CallbackContext context)
        {
            _worldUI.MoveUp();
            _targetSelector.MoveUp();
        }
        
        private void OnDownPerformed(InputAction.CallbackContext context)
        {
            _worldUI.MoveDown();
            _targetSelector.MoveDown();
        }
        
        private void OnLeftPerformed(InputAction.CallbackContext context) => _targetSelector.MoveLeft();

        private void OnRightPerformed(InputAction.CallbackContext context) => _targetSelector.MoveRight();
        
        private void OnSelectPerformed(InputAction.CallbackContext context) => _worldUI.Select(Enums.Input.A);

        private void OnSkillPerformed(InputAction.CallbackContext context) => _worldUI.Select(Enums.Input.Y);
        
        private void OnItemsPerformed(InputAction.CallbackContext context) => _worldUI.Select(Enums.Input.X);

        private void OnGuardPerformed(InputAction.CallbackContext context) => _worldUI.Select(Enums.Input.B);

        private IReadOnlyList<IListedItemData> GetItemsData(Enums.ListedItem item)
        {
            switch (item)
            {
                case Enums.ListedItem.Item:
                    return null;
                
                case Enums.ListedItem.BattleAction:
                    return _battleUnitsHandler.ActiveUnit.SkillsHandler.GetSkillsOfType<IActiveSkillData>();
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }
        
        private void OnActionSelected(Enums.Input action, int index)
        {
            Skill concreteAction;
            
            switch (action)
            {
                case Enums.Input.A:
                    concreteAction = _battleUnitsHandler.ActiveUnit.SkillsHandler.GetBasicAttack();
                    ToActionExecutionState(concreteAction);
                    break;
                
                case Enums.Input.Y:
                    concreteAction = _battleUnitsHandler.ActiveUnit.SkillsHandler.GetAction(index);
                    ToActionExecutionState(concreteAction);
                    break;
                
                case Enums.Input.B:
                    ToNextState<TurnSelectionState>().Forget();
                    break;
                
                case Enums.Input.X:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        private async UniTask ToNextState<T>() where T: IBattleState
        {
            await DeactivateAsync();
            
            stateChanger.ChangeState<T>();
        }

        private void ToActionExecutionState(Skill action)
        {
            _args.SetAction(action);
            
            ToNextState<ActionExecutionState>().Forget();
        }
    }
}