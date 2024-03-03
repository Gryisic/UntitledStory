using System;
using System.Collections.Generic;
using System.Threading;
using Common.Battle.Interfaces;
using Common.Battle.Utils;
using Common.Navigation;
using Common.UI.Battle;
using Common.Units.Battle;
using Common.Units.Handlers;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Battle.States
{
    public class BattleFinalizeState : BattleStateBase, IGameStateChangeRequester, IBattleStateArgsRequester, IConcreteStateResetRequester, IResettable, IDisposable
    {
        private readonly BattleUnitsHandler _unitsHandler;
        private readonly ConstraintsFinalizer _constraintsFinalizer;
        
        private readonly BattleOverlayView _overlayUI;

        private CancellationTokenSource _finalizeTokenSource;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;
        public event Func<BattleStateArgs> RequestArgs;
        public event Action StateResetRequested;

        public BattleFinalizeState(IStateChanger<IBattleState> stateChanger, BattleUnitsHandler unitsHandler, UI.UI ui) : base(stateChanger)
        {
            _unitsHandler = unitsHandler;
            _overlayUI = ui.Get<BattleOverlayView>();
            
            _constraintsFinalizer = new ConstraintsFinalizer();
        }
        
        public void Dispose()
        {
            _finalizeTokenSource?.Cancel();
            _finalizeTokenSource?.Dispose();
        }

        public override void Activate()
        {
            _finalizeTokenSource = new CancellationTokenSource();
            
            FinalizeAsync().Forget();
        }

        public void Reset()
        {
            _unitsHandler.Clear();
        }

        private async UniTask FinalizeAsync()
        {
            BattleStateArgs args = RequestArgs?.Invoke();
            
            if (args.Constraints != null)
                _constraintsFinalizer.Finalize(args.Constraints);

            await _overlayUI.DeactivateAsync(_finalizeTokenSource.Token);
            await ReturnUnitToStartPoint(_unitsHandler.PartyMembers[0], args);
            
            _unitsHandler.DeactivateAll();

            StateResetRequested?.Invoke();
            RequestStateChange?.Invoke(Enums.GameStateType.Explore, new ExploringStateArgs(args.StartPoint));
        }

        private async UniTask ReturnUnitToStartPoint(BattleUnit unit, BattleStateArgs args)
        {
            bool isPathExist = args.NavigationArea.TryGetPath(unit.Transform.position, args.StartPoint, out IReadOnlyList<NavigationCell> path);

            if (isPathExist)
            {
                foreach (var cell in path) 
                    unit.MoveToPoint(cell.Position);
            
                await unit.ExecuteActionsWithAwaiter();
            }
            
            args.UpdateStartPoint(unit.Transform.position);
        }
    }
}