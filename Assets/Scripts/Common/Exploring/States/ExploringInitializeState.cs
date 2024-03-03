using Common.Exploring.Interfaces;
using Common.Models.Scene;
using Common.Units.Exploring;
using Common.Units.Handlers;
using Common.Units.Templates;
using Core;
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

        public ExploringInitializeState(IStateChanger<IExploringState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Player player, SceneInfo sceneInfo, ExploringUnitsHandler exploringUnitsHandler)
        {
            _stateChanger = stateChanger;
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
            
            ExploringUnit unit = _exploringUnitsHandler.GetUnitWithID(unitTemplate.ID);
            
            unit.Initialize(unitTemplate);
            _player.UpdateExploringUnit(unit);
        }
    }
}