using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.GameStates;
using Core.Interfaces;
using Infrastructure.Factories.GameStatesFactory.Interfaces;
using UnityEngine;
using Zenject;

namespace Core
{
    public class Game : MonoBehaviour, IDisposable, IGameStateSwitcher
    {
        private IReadOnlyList<IGameState> _gameStates;
        private IGameStatesFactory _factory;
        
        private IGameState _currentState;
        
        [Inject]
        private void Construct(IGameStatesFactory factory)
        {
            _factory = factory;
        }

        public void Initiate()
        {
            CreateStates(_factory);

            foreach (var gameState in _gameStates)
            {
                if (gameState is IStatesResetRequester resetRequester)
                    resetRequester.ResetRequested += ResetAllStates;
            }
            
            SwitchState<GameInitializeState>(new GameStateArgs());
        }

        public void Dispose()
        {
            _currentState.Deactivate();
            
            foreach (var gameState in _gameStates)
            {
                if (gameState is IStatesResetRequester resetRequester)
                    resetRequester.ResetRequested -= ResetAllStates;
            }
        }

        public void SwitchState<T>(GameStateArgs args) where T : IGameState
        {
            _currentState?.Deactivate();
            _currentState = _gameStates.First(s => s is T);
            _currentState.Activate(args);
        }

        private void CreateStates(IGameStatesFactory factory)
        {
            factory.CreateAllStates();

            _gameStates = factory.GameModes;
        }
        
        private void ResetAllStates()
        {
            foreach (var gameState in _gameStates)
            {
                gameState.Reset();
            }
        }
    }
}
