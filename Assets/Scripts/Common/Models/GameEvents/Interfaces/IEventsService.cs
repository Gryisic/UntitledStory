using System.Collections.Generic;
using Core.Interfaces;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IEventsService : IService
    {
        IReadOnlyList<IGameEvent> Events { get; }

        void AddEvent(IGameEvent gameEvent);

        void AddEvents(IReadOnlyList<IGameEvent> gameEvents);

        void RemoveEvent(IGameEvent gameEvent);

        void RemoveEvents(IReadOnlyList<IGameEvent> gameEvents);
    }
}