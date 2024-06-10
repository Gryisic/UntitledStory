using System;
using Common.Battle.Interfaces;
using Common.Battle.States;
using Common.Models.GameEvents.Interfaces;
using Core.Extensions;
using Core.Interfaces;
using Infrastructure.Factories.BattleStatesFactory.Interfaces;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class BattleState : StatesChanger<IBattleState>, IGameState, IDeactivatable, IResettable, IDisposable
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly IBattleStateFactory _stateFactory;

        private BattleStateArgs _args;
        
        private bool _isInitialized;
        
        public BattleState(IGameStateSwitcher stateSwitcher, IBattleStateFactory stateFactory)
        {
            _stateSwitcher = stateSwitcher;
            _stateFactory = stateFactory;
        }
        
        public void Dispose()
        {
            
        }
        
        public void Activate(GameStateArgs args)
        {
            if (args is BattleStateArgs battleArgs == false)
                throw new InvalidOperationException("Trying to initiate battle via non BattleStateArgs");

            _args = battleArgs;
            
            if (_isInitialized == false)
            {
                CreateStates();
                SubscribeToEvents();
                ChangeState<BattleInitializeState>();
                
                _isInitialized = true;

                return;
            }

            SubscribeToEvents();
            ChangeState<PartyTurnState>();
        }

        public void Deactivate()
        {
            UnsubscribeToEvents();
            
            currentState.Deactivate();
        }

        public void Reset()
        {
            _isInitialized = false;
            
            if (states == null)
                return;

            foreach (var state in states) 
                state.Reset();
        }
        
        private BattleStateArgs GetArgs() => _args;

        private void CreateStates()
        {
            _stateFactory.CreateAllStates();

            states = _stateFactory.States;
        }
        
        private void SubscribeToEvents()
        {
            foreach (var state in states)
            {
                if (state is IGameStateChangeRequester stateChangeRequester)
                    stateChangeRequester.RequestStateChange += ToNextState;
                
                if (state is IBattleStateArgsRequester argsRequester)
                    argsRequester.RequestArgs += GetArgs;
                
                if (state is IConcreteStateResetRequester stateResetRequester)
                    stateResetRequester.StateResetRequested += OnStateResetRequested;
                
                if (state is IBattleEndRequester endRequester)
                    endRequester.RequestBattleEnd += OnBattleEndRequested;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var state in states)
            {
                if (state is IGameStateChangeRequester stateChangeRequester)
                    stateChangeRequester.RequestStateChange -= ToNextState;
                
                if (state is IBattleStateArgsRequester argsRequester)
                    argsRequester.RequestArgs -= GetArgs;
                
                if (state is IConcreteStateResetRequester stateResetRequester)
                    stateResetRequester.StateResetRequested -= OnStateResetRequested;
                
                if (state is IBattleEndRequester endRequester)
                    endRequester.RequestBattleEnd -= OnBattleEndRequested;
            }
        }
        
        private void ToNextState(Enums.GameStateType nextState, GameStateArgs args)
        {
            switch (nextState)
            {
                case Enums.GameStateType.Dialogue:
                    _stateSwitcher.SwitchState<DialogueState>(args);
                    break;
                
                case Enums.GameStateType.Explore:
                    _stateSwitcher.SwitchState<ExploringState>(args);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
            }
        }
        
        private void OnBattleEndRequested(Enums.BattleResult result)
        {
            switch (result)
            {
                case Enums.BattleResult.Win:
                    ChangeState<BattleFinalizeState>();
                    return;
                
                case Enums.BattleResult.Lose:
                    throw new NotImplementedException("Battle lose state is not implemented yet!");
                
                case Enums.BattleResult.Event:
                    throw new NotImplementedException("Battle end with event state is not implemented yet!");
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        private void OnStateResetRequested() => Reset();
    }
}