using Core.Interfaces;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class GameInitializeState : IGameState
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        
        public GameInitializeState(IGameStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        
        public void Activate(GameStateArgs args)
        {
            _stateSwitcher.SwitchState<SceneSwitchState>(new SceneSwitchArgs(Enums.SceneType.Academy, Enums.GameStateType.Explore));
        }
    }
}