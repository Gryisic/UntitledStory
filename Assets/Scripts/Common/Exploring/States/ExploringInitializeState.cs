using Common.Exploring.Interfaces;
using Common.Models.Scene;
using Common.Units;
using Common.Units.Exploring;
using Common.Units.Handlers;
using Common.Units.Templates;
using Core;
using Core.Data.Interfaces;
using Core.Interfaces;
using Infrastructure.Factories.UnitsFactory.Interfaces;

namespace Common.Exploring.States
{
    public class ExploringInitializeState : IExploringState
    {
        private readonly IStateChanger<IExploringState> _stateChanger;
        private readonly IUnitFactory _unitFactory;
        private readonly IServicesHandler _servicesHandler;
        private readonly IGameDataProvider _gameDataProvider;
        
        private readonly Player _player;
        private readonly SceneInfo _sceneInfo;
        private readonly ExploringUnitsHandler _exploringUnitsHandler;

        public ExploringInitializeState(IStateChanger<IExploringState> stateChanger, IUnitFactory unitFactory, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Player player, SceneInfo sceneInfo, ExploringUnitsHandler exploringUnitsHandler)
        {
            _stateChanger = stateChanger;
            _unitFactory = unitFactory;
            _servicesHandler = servicesHandler;
            _gameDataProvider = gameDataProvider;
            _player = player;
            _sceneInfo = sceneInfo;
            _exploringUnitsHandler = exploringUnitsHandler;
        }

        public void Activate()
        {
            CreatePartyUnits();
            
            _stateChanger.ChangeState<ExploringActiveState>();
        }

        private void CreatePartyUnits()
        {
            IPartyData data = _gameDataProvider.GetData<IPartyData>();
            ExploringUnitTemplate unitTemplate = data.ExploringUnitsTemplates[0];
            
            _unitFactory.Load(unitTemplate.ID);
            
            ExploringUnit unit = _unitFactory.Create(unitTemplate, _sceneInfo.ExploreUnitSpawnPoint.position) as ExploringUnit;
            
            unit.Initialize(unitTemplate);
            _exploringUnitsHandler.Add(unit);
            _player.UpdateExploringUnit(unit);
        }
    }
}