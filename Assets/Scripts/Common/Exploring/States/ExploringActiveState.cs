using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exploring.Interfaces;
using Common.Models.Cameras.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Scene;
using Common.Models.Triggers.Interfaces;
using Common.Units.Handlers;
using Core;
using Core.Data.Interfaces;
using Core.GameStates;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Exploring.States
{
    public class ExploringActiveState : IExploringState, IDeactivatable, IGameStateChangeRequester, IGameStateArgsRequester<ExploringStateArgs>
    {
        private readonly IStateChanger<IExploringState> _stateChanger;
        private readonly IInputService _inputService;
        private readonly ICameraService _cameraService;
        private readonly IEventsService _eventsService;
        private readonly ITriggersData _triggersData;
        
        private readonly Player _player;
        private readonly SceneInfo _sceneInfo;
        private readonly ExploringUnitsHandler _unitsHandler;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;
        public event Func<ExploringStateArgs> RequestArgs;

        public ExploringActiveState(IStateChanger<IExploringState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Player player, SceneInfo sceneInfo, ExploringUnitsHandler unitsHandler)
        {
            _stateChanger = stateChanger;
            _inputService = servicesHandler.InputService;
            _eventsService = servicesHandler.EventsService;
            _cameraService = servicesHandler.GetSubService<ICameraService>();
            _triggersData = gameDataProvider.GetData<ITriggersData>();
            _player = player;
            _sceneInfo = sceneInfo;
            _unitsHandler = unitsHandler;
        }
        
        public void Activate()
        {
            _eventsService.AddEvents(_sceneInfo.MonoTriggersHandler.Triggers);
            
            ValidateData();
            ValidateUnits();
            
            SubscribeToEvents();
            AttachInput();
            
            _player.Activate();
            _cameraService.FollowUnit(_unitsHandler.ActiveUnit.transform);
        }

        public void Deactivate()
        {
            _player.Deactivate();
            
            DeAttachInput();
            UnsubscribeToEvents();
            
            _eventsService.RemoveEvents(_sceneInfo.MonoTriggersHandler.Triggers);
        }

        private void SubscribeToEvents()
        {
            foreach (var gameEvent in _eventsService.Events)
            {
                if (gameEvent is IMonoTrigger monoTrigger)
                    monoTrigger.IDUsed += _triggersData.Remove;
                
                if (gameEvent is IGameStateChangerEvent gameStateChanger)
                    gameStateChanger.StateChangeRequested += OnGameStateChangeRequested;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var gameEvent in _eventsService.Events)
            {
                if (gameEvent is IMonoTrigger monoTrigger)
                    monoTrigger.IDUsed -= _triggersData.Remove;
                
                if (gameEvent is IGameStateChangerEvent gameStateChanger)
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
            if (nextState == Enums.GameStateType.Battle)
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

        private void ValidateUnits()
        {
            if (_unitsHandler.ActiveUnit.isActiveAndEnabled)
                return;
            
            ExploringStateArgs args = RequestArgs?.Invoke();
            
            _unitsHandler.RestoreActiveUnitAtPosition(args.Position);
        }
    }
}