using System;
using Core.GameStates;
using Infrastructure.Utils;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IGameStateChangerEvent : IGameEvent
    {
        event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;
    }
}