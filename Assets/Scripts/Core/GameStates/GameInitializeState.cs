using System;
using Common.Models.GameEvents.Interfaces;
using Core.Data.Icons;
using Core.Data.Interfaces;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class GameInitializeState : IGameState
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly IServicesHandler _servicesHandler;
        private readonly IGameDataProvider _gameDataProvider;
        
        public GameInitializeState(IGameStateSwitcher stateSwitcher, IServicesHandler servicesHandler, IGameDataProvider dataProvider)
        {
            _stateSwitcher = stateSwitcher;
            _servicesHandler = servicesHandler;
            _gameDataProvider = dataProvider;
        }
        
        public void Activate(GameStateArgs args)
        {
            _gameDataProvider.GetData<IIconsData>().GetIcons<InputIcons>().Initialize(_servicesHandler.InputService);

            SubscribeToEvents();
            
            _stateSwitcher.SwitchState<SceneSwitchState>(new SceneSwitchArgs(Enums.SceneType.Academy, Enums.GameStateType.Explore));
        }
        
        private void SubscribeToEvents()
        {
            _servicesHandler.InputService.DeviceChanged += _gameDataProvider.GetData<IIconsData>().GetIcons<InputIcons>().UpdateActiveMap;
        }
    }
}