using System;
using Core.GameStates;
using Infrastructure.Utils;

namespace Core.Interfaces
{
    public interface IGameStateChangeRequester
    {
        event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;
    }
}