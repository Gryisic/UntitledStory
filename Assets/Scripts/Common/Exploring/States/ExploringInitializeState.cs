using Common.Exploring.Interfaces;
using Common.Models.GameEvents;
using Common.Models.Scene;
using Common.Models.Triggers;
using Common.Units;
using Common.Units.Exploring;
using Common.Units.Handlers;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Core;
using Core.Configs.Interfaces;
using Core.Data.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Common.Exploring.States
{
    public class ExploringInitializeState : IExploringState
    {
        private readonly IStateChanger<IExploringState> _stateChanger;
        private readonly IServicesHandler _servicesHandler;
        private readonly IGameDataProvider _gameDataProvider;
        
        private readonly Player _player;
        private readonly SceneInfo _sceneInfo;
        private readonly GeneralUnitsHandler _generalUnitsHandler;

        private readonly IUnitsConfig _unitsConfig;
        
        public ExploringInitializeState(IStateChanger<IExploringState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Player player, SceneInfo sceneInfo, GeneralUnitsHandler generalUnitsHandler)
        {
            _stateChanger = stateChanger;
            _servicesHandler = servicesHandler;
            _gameDataProvider = gameDataProvider;
            _player = player;
            _sceneInfo = sceneInfo;
            _generalUnitsHandler = generalUnitsHandler;
            _unitsConfig = _servicesHandler.ConfigsService.GetConfig<IUnitsConfig>();
        }

        public void Activate()
        {
            CreatePartyUnits();
            InitializeTriggers();
            
            _stateChanger.ChangeState<ExploringActiveState>();
        }

        private void CreatePartyUnits()
        {
            IPartyData data = _gameDataProvider.GetData<IPartyData>();
            PartyMemberTemplate unitTemplate = data.Templates[0];
            
            IUnit unit = _generalUnitsHandler.GetUnitWithID(unitTemplate.ID);
            
            unit.Initialize(unitTemplate);
            _player.UpdateActionsExecutor(unit as IExploringActionsExecutor);
        }

        private void InitializeTriggers()
        {
            foreach (var triggerZone in _sceneInfo.MonoTriggersHandler.TriggerZones) 
                triggerZone.Initialize(new TriggerInitializationArgs(_generalUnitsHandler));
        }
    }
}