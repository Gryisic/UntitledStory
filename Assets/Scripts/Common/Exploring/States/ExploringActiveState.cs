using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exploring.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Scene;
using Common.Units.Handlers;
using Core;
using Core.Data.Interfaces;
using Core.GameStates;
using Core.Interfaces;
using Infrastructure.Utils;

namespace Common.Exploring.States
{
    public class ExploringActiveState : IExploringState, IDeactivatable, IGameStateChangeRequester
    {
        private readonly IStateChanger<IExploringState> _stateChanger;
        private readonly IInputService _inputService;
        private readonly ITriggersData _triggersData;
        
        private readonly Player _player;
        private readonly SceneInfo _sceneInfo;
        private readonly ExploringUnitsHandler _unitsHandler;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;

        public ExploringActiveState(IStateChanger<IExploringState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Player player, SceneInfo sceneInfo, ExploringUnitsHandler unitsHandler)
        {
            _stateChanger = stateChanger;
            _inputService = servicesHandler.InputService;
            _triggersData = gameDataProvider.GetData<ITriggersData>();
            _player = player;
            _sceneInfo = sceneInfo;
            _unitsHandler = unitsHandler;
        }
        
        public void Activate()
        {
            ValidateData();
            
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
                trigger.IDUsed += _triggersData.Remove;

                if (trigger is IGameStateChangerEvent gameStateChanger)
                    gameStateChanger.StateChangeRequested += OnGameStateChangeRequested;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var trigger in _sceneInfo.MonoTriggersHandler.Triggers)
            {
                trigger.IDUsed -= _triggersData.Remove;
                
                if (trigger is IGameStateChangerEvent gameStateChanger)
                    gameStateChanger.StateChangeRequested -= OnGameStateChangeRequested;
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
        
        private void OnGameStateChangeRequested(Enums.GameStateType nextState, GameStateArgs args)
        {
            _unitsHandler.DeactivateAll();
            
            RequestStateChange?.Invoke(nextState, args);
        }

        private void ValidateData()
        {
            if (_triggersData.IsDirty == false)
                return;
            
            List<string> ids = _triggersData.GetIDList().ToList();

            foreach (var trigger in _sceneInfo.MonoTriggersHandler.Triggers)
            {
                List<string> intersectedIDs = trigger.IDs.Intersect(ids).ToList();
                    
                trigger.SetActiveIDs(intersectedIDs);
                ids = ids.Except(intersectedIDs).ToList();
            }
        }
    }
}