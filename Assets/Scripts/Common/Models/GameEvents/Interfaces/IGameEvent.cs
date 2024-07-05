using System;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IGameEvent : IGameEventData
    {
        event Action<IGameEvent> Ended;
    }
}