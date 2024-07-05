using Common.Models.GameEvents.Interfaces;
using Common.Models.Scene;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class SceneSwitchArgs : GameStateArgs
    {
        public Enums.SceneType NextSceneType { get; }
        public Enums.GameStateType NextGameState { get; }
        public SceneInfo CurrentSceneInfo { get; }

        public SceneSwitchArgs(Enums.SceneType nextSceneType, Enums.GameStateType nextGameState = Enums.GameStateType.Explore, SceneInfo currentSceneInfo = null, IGameEventData eventData = null) : base(eventData)
        {
            NextSceneType = nextSceneType;
            NextGameState = nextGameState;
            CurrentSceneInfo = currentSceneInfo;
        }
    }
}