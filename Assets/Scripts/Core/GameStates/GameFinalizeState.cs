using Core.Data.Icons;
using Core.Data.Interfaces;
using Core.Interfaces;

namespace Core.GameStates
{
    public class GameFinalizeState : IGameState
    {
        private readonly IServicesHandler _servicesHandler;
        private readonly IGameDataProvider _gameDataProvider;

        public GameFinalizeState(IServicesHandler servicesHandler, IGameDataProvider dataProvider)
        {
            _servicesHandler = servicesHandler;
            _gameDataProvider = dataProvider;   
        }
        
        public void Activate(GameStateArgs args)
        {
            FinalizeGame();
        }

        private void FinalizeGame()
        {
            UnsubscribeToEvents();
        }

        private void UnsubscribeToEvents()
        {
            _servicesHandler.InputService.DeviceChanged -= _gameDataProvider.GetData<IIconsData>().GetIcons<InputIcons>().UpdateActiveMap;
        }
    }
}