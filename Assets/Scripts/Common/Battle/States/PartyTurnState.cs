using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Interfaces;
using Common.UI.Battle;
using Common.UI.Extensions;
using Common.UI.Interfaces;
using Common.Units.Handlers;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine.InputSystem;

namespace Common.Battle.States
{
    public class PartyTurnState : BattleStateBase, IDeactivatable, IResettable, IDisposable, IGameStateChangeRequester
    {
        private readonly IInputService _inputService;

        private readonly BattleUnitsHandler _battleUnitsHandler;
        private readonly BattleActionsView _ui;

        private CancellationTokenSource _uiAnimationTokenSource;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;
        
        public PartyTurnState(IStateChanger<IBattleState> stateChanger, IInputService inputService, BattleUnitsHandler battleUnitsHandler, UI.UI ui) : base(stateChanger)
        {
            _inputService = inputService;
            _battleUnitsHandler = battleUnitsHandler;
            _ui = ui.Get<BattleActionsView>();
        }
        
        public void Dispose()
        {
            _uiAnimationTokenSource?.Cancel();
            _uiAnimationTokenSource?.Dispose();
        }

        public override void Activate()
        {
            AttachInput();
            SubscribeToEvents();

            _uiAnimationTokenSource = new CancellationTokenSource();
            
            _ui.ActivateAsync(_uiAnimationTokenSource.Token).Forget();
        }
        
        public void Deactivate()
        {
            UnsubscribeToEvents();
            DeAttachInput();
            
            _ui.DeactivateAsync(_uiAnimationTokenSource.Token).Forget();
        }
        
        public void Reset()
        {
            _battleUnitsHandler.Clear();
        }

        private void SubscribeToEvents()
        {
            _ui.RequestItemsData += GetItemsData;
            _ui.ActionSelected += OnActionSelected;
        }

        private void UnsubscribeToEvents()
        {
            _ui.RequestItemsData -= GetItemsData;
        }
        
        private void AttachInput()
        {
            _inputService.Input.Battle.AttackSelect.performed += OnSelectPerformed;
            _inputService.Input.Battle.Skill.performed += OnSkillPerformed;
            _inputService.Input.Battle.GuardCancel.performed += OnGuardPerformed;
            _inputService.Input.Battle.Items.performed += OnItemsPerformed;
            _inputService.Input.Battle.Up.performed += _ui.MoveUp;
            _inputService.Input.Battle.Down.performed += _ui.MoveDown;

            _inputService.Input.Battle.Enable();
        }

        private void DeAttachInput()
        {
            _inputService.Input.Battle.Disable();
            
            _inputService.Input.Battle.AttackSelect.performed -= OnSelectPerformed;
            _inputService.Input.Battle.Skill.performed -= OnSkillPerformed;
            _inputService.Input.Battle.GuardCancel.performed -= OnGuardPerformed;
            _inputService.Input.Battle.Items.performed -= OnItemsPerformed;
            _inputService.Input.Battle.Up.performed -= _ui.MoveUp;
            _inputService.Input.Battle.Down.performed -= _ui.MoveDown;
        }

        private void OnSelectPerformed(InputAction.CallbackContext context) => _ui.Select(Enums.BattleActions.Attack);

        private void OnSkillPerformed(InputAction.CallbackContext context) => _ui.Select(Enums.BattleActions.Skill);
        
        private void OnItemsPerformed(InputAction.CallbackContext context) => _ui.Select(Enums.BattleActions.Items);

        private void OnGuardPerformed(InputAction.CallbackContext obj)
        {
            _ui.Select(Enums.BattleActions.Guard);
            _ui.Cancel();
        }
        
        private IReadOnlyList<IListedItemData> GetItemsData(Enums.ListedItem item)
        {
            switch (item)
            {
                case Enums.ListedItem.Item:
                    return null;
                
                case Enums.ListedItem.BattleAction:
                    return _battleUnitsHandler.ActiveUnit.ActionsHandler.ExposedData.ToList();
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }
        
        private void OnActionSelected(Enums.BattleActions action, int index)
        {
            switch (action)
            {
                case Enums.BattleActions.Attack:
                    ToNextState();
                    break;
                
                case Enums.BattleActions.Skill:
                    break;
                
                case Enums.BattleActions.Guard:
                    ToNextState<TurnSelectionState>();
                    break;
                
                case Enums.BattleActions.Items:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        private void ToNextState<T>() where T: IBattleState => stateChanger.ChangeState<T>();
        
        private void ToNextState() => ToNextState<ActionExecutionState>();
    }
}