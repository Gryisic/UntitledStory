using System;
using Common.Exploring.Interfaces;
using Common.Exploring.States;
using Common.UI;
using Core.Extensions;
using Core.Interfaces;
using Infrastructure.Factories.ExploringStateFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class ExploringState : StatesChanger<IExploringState>, IGameState, IDeactivatable, IResettable, IDisposable
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly IExploringStateFactory _exploringStateFactory;

        private ExploringStateArgs _args;
        
        private bool _isInitialized;

        public ExploringState(IGameStateSwitcher stateSwitcher, IExploringStateFactory exploringStateFactory, UI ui)
        {
            _stateSwitcher = stateSwitcher;
            _exploringStateFactory = exploringStateFactory;
        }

        public void Dispose()
        {
            
        }
        
        public void Activate(GameStateArgs args)
        {
            if (args is ExploringStateArgs exploringArgs == false)
                throw new InvalidOperationException("Trying to initiate exploring via non ExploringStateArgs");

            _args = exploringArgs;
            
            if (_isInitialized == false)
            {
                CreateStates();
                SubscribeToEvents();
                ChangeState<ExploringInitializeState>();

                _isInitialized = true;
                
                return;
            }
            
            SubscribeToEvents();
            ChangeState<ExploringActiveState>();
        }

        public void Deactivate()
        {
            UnsubscribeToEvents();
            
            currentState.Deactivate();
        }
        
        public void Reset()
        {
            _isInitialized = false;
        }
        
        private ExploringStateArgs GetArgs() => _args;

        private void CreateStates()
        {
            _exploringStateFactory.CreateAllStates();

            states = _exploringStateFactory.States;
        }
        
        private void SubscribeToEvents()
        {
            foreach (var state in states)
            {
                if (state is IGameStateChangeRequester stateChangeRequester)
                    stateChangeRequester.RequestStateChange += ToNextState;
                
                if (state is IGameStateArgsRequester<ExploringStateArgs> argsRequester)
                    argsRequester.RequestArgs += GetArgs;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var state in states)
            {
                if (state is IGameStateChangeRequester stateChangeRequester)
                    stateChangeRequester.RequestStateChange -= ToNextState;
                
                if (state is IGameStateArgsRequester<ExploringStateArgs> argsRequester)
                    argsRequester.RequestArgs -= GetArgs;
            }
        }
        
        private void ToNextState(Enums.GameStateType nextState, GameStateArgs args)
        {
            switch (nextState)
            {
                case Enums.GameStateType.SceneSwitch:
                    _stateSwitcher.SwitchState<SceneSwitchState>(args);
                    break;
                
                case Enums.GameStateType.Dialogue:
                    _stateSwitcher.SwitchState<DialogueState>(args);
                    break;
                
                case Enums.GameStateType.Battle:
                    _stateSwitcher.SwitchState<BattleState>(args);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
            }
        }
    }
}