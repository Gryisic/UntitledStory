﻿using Infrastructure.Utils;

namespace Core.GameStates
{
    public class SceneSwitchArgs : GameStateArgs
    {
        public Enums.SceneType NextSceneType { get; }
        public Enums.GameStateType NextGameState { get; }

        public SceneSwitchArgs(Enums.SceneType nextSceneType, Enums.GameStateType nextGameState = Enums.GameStateType.Explore)
        {
            NextSceneType = nextSceneType;
            NextGameState = nextGameState;
        }
    }
}