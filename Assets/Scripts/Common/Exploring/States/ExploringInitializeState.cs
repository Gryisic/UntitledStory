using Common.Exploring.Interfaces;
using Common.Models.Scene;
using Common.Units;
using Common.Units.Exploring;
using Common.Units.Handlers;
using Common.Units.Placement;
using Common.Units.Templates;
using Core;
using Core.Configs.Interfaces;
using Core.Data.Interfaces;
using Core.Interfaces;

namespace Common.Exploring.States
{
    public class ExploringInitializeState : IExploringState
    {
        private readonly IStateChanger<IExploringState> _stateChanger;
        private readonly IServicesHandler _servicesHandler;
        private readonly IGameDataProvider _gameDataProvider;
        
        private readonly Player _player;
        private readonly SceneInfo _sceneInfo;
        private readonly ExploringUnitsHandler _exploringUnitsHandler;

        private readonly IUnitsConfig _unitsConfig;
        
        public ExploringInitializeState(IStateChanger<IExploringState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Player player, SceneInfo sceneInfo, ExploringUnitsHandler exploringUnitsHandler)
        {
            _stateChanger = stateChanger;
            _servicesHandler = servicesHandler;
            _gameDataProvider = gameDataProvider;
            _player = player;
            _sceneInfo = sceneInfo;
            _exploringUnitsHandler = exploringUnitsHandler;
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
            ExploringUnitTemplate unitTemplate = data.ExploringUnitsTemplates[0];
            
            ExploringUnit unit = _exploringUnitsHandler.GetUnitWithID(unitTemplate.ID);
            
            unit.Initialize(unitTemplate);
            _player.UpdateExploringUnit(unit);
        }

        private void InitializeTriggers()
        {
            foreach (var triggerZone in _sceneInfo.MonoTriggersHandler.TriggerZones) 
                triggerZone.Initialize();
        }

        private void CreateUnit(UnitPlace place)
        {
            UnitTemplate template = _unitsConfig.GetTemplateWithID(place.ID);
            Unit unit = _exploringUnitsHandler.GetUnitWithID(template.ID);

            unit.Initialize(template);
        }
    }
}