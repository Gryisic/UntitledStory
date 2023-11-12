using System;
using Common.Exploring.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Scene;
using Core;
using Core.GameStates;
using Core.Interfaces;
using Infrastructure.Utils;

namespace Common.Exploring.States
{
    public class ExploringActiveState : IExploringState, IDeactivatable, IGameStateChangeRequester
    {
        private readonly IStateChanger<IExploringState> _stateChanger;
        private readonly IInputService _inputService;
        
        private readonly Player _player;
        private readonly SceneInfo _sceneInfo;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;

        public ExploringActiveState(IStateChanger<IExploringState> stateChanger, IServicesHandler servicesHandler, Player player, SceneInfo sceneInfo)
        {
            _stateChanger = stateChanger;
            _inputService = servicesHandler.InputService;
            _player = player;
            _sceneInfo = sceneInfo;
        }
        
        public void Activate()
        {
            SubscribeToEvents();
            AttachInput();
            
            _player.Activate();
        }

        public void Deactivate()
        {
            _player.Deactivate();
            
            DeAttachInput();
            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            foreach (var trigger in _sceneInfo.MonoTriggersHandler.Triggers)
            {
                if (trigger is IGameStateChangerEvent gameStateChanger)
                    gameStateChanger.RequestStateChange += OnGameStateChangeRequested;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var trigger in _sceneInfo.MonoTriggersHandler.Triggers)
            {
                if (trigger is IGameStateChangerEvent gameStateChanger)
                    gameStateChanger.RequestStateChange -= OnGameStateChangeRequested;
            }
        }
        
        private void AttachInput()
        {
            _inputService.Input.Explore.Move.performed += _player.StartMoving;
            _inputService.Input.Explore.Move.canceled += _player.StopMoving;
            _inputService.Input.Explore.Attack.performed += _player.Attack;
            _inputService.Input.Explore.Interact.performed += _player.Interact;
            
            _inputService.Input.Explore.Enable();
        }
        
        private void DeAttachInput()
        {
            _inputService.Input.Explore.Disable();
            
            _inputService.Input.Explore.Move.performed -= _player.StartMoving;
            _inputService.Input.Explore.Move.canceled -= _player.StopMoving;
            _inputService.Input.Explore.Attack.performed -= _player.Attack;
            _inputService.Input.Explore.Interact.performed -= _player.Interact;
        }
        
        private void OnGameStateChangeRequested(Enums.GameStateType nextState, GameStateArgs args) => RequestStateChange?.Invoke(nextState,args);
    }
}