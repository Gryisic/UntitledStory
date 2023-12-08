using System;
using Common.Battle.States;
using Common.UI;
using Core.Configs;
using Core.Configs.Interfaces;
using Core.Data;
using Core.Data.Interfaces;
using Core.GameStates;
using Core.Interfaces;
using Core.PlayerInput;
using Core.Utils;
using Infrastructure.Factories.BattleStatesFactory;
using Infrastructure.Factories.BattleStatesFactory.Interfaces;
using Infrastructure.Factories.DialogueStatesFactory;
using Infrastructure.Factories.DialogueStatesFactory.Interfaces;
using Infrastructure.Factories.ExploringStateFactory;
using Infrastructure.Factories.ExploringStateFactory.Interfaces;
using Infrastructure.Factories.GameStatesFactory;
using Infrastructure.Factories.GameStatesFactory.Interfaces;
using UnityEngine;
using Zenject;
using Input = Core.PlayerInput.Input;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private Game _game;
        [SerializeField] private ConfigsService _configsService;
        [SerializeField] private GameDataProvider _gameDataProvider;
        [SerializeField] private UI _ui;

        public void Initialize()
        {
            IServicesHandler services = Container.Resolve<IServicesHandler>();
            
            _gameDataProvider.Initialize(services);
            _game.Initiate();
        }

        public override void InstallBindings()
        {
            BindDataProvider();
            BindServices();
            BindSceneSwitcher();
            BindFactories();
            BindGameStates();
            BindUI();
            BindGame();
            BindSelf();
        }

        private void BindSelf() => Container.BindInterfacesAndSelfTo<GameInstaller>().FromInstance(this).AsSingle();

        private void BindSceneSwitcher() => Container.Bind<SceneSwitcher>().AsSingle();
        
        private void BindDataProvider() => Container.Bind<IGameDataProvider>().To<GameDataProvider>().FromInstance(_gameDataProvider).AsSingle();

        private void BindServices()
        {
            Container.Bind<Input>().WhenInjectedInto<InputService>();
            Container.BindInterfacesTo<InputService>().AsSingle();

            Container.Bind<IConfigsService>().To<ConfigsService>().FromInstance(_configsService).AsSingle();
            
            Container.BindInterfacesTo<ServicesHandler>().AsSingle().CopyIntoDirectSubContainers();
        }
        
        private void BindUI()
        {
            _ui = Container.InstantiatePrefabForComponent<UI>(_ui);
            
            Container.Bind<UI>().FromInstance(_ui).AsSingle();
        }
        
        private void BindGame()
        {
            _game = Container.InstantiatePrefabForComponent<Game>(_game);

            Container.BindInterfacesTo<Game>().FromInstance(_game).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IExploringStateFactory>().To<ExploringStatesFactory>().AsSingle();
            Container.Bind<IDialogueStatesFactory>().To<DialogueStatesFactory>().AsSingle();
            Container.Bind<IBattleStateFactory>().To<BattleStateFactory>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<DialogueStatesFactoryUpdater>().AsSingle().CopyIntoDirectSubContainers();
            Container.BindInterfacesAndSelfTo<BattleStateFactoryUpdater>().AsSingle().CopyIntoDirectSubContainers();
            Container.BindInterfacesAndSelfTo<ExploringStatesFactoryUpdater>().AsSingle().CopyIntoDirectSubContainers();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<GameInitializeState>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneSwitchState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogueState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExploringState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleState>().AsSingle();

            Container.Bind<IGameStatesFactory>().To<GameStatesFactory>().AsSingle();
        }
    }
}