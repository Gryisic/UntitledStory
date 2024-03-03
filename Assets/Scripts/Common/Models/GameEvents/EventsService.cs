using System;
using System.Collections.Generic;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents
{
    public class EventsService : IEventsService
    {
        private readonly List<IGameEvent> _events;

        public IReadOnlyList<IGameEvent> Events => _events;

        public EventsService()
        {
            _events = new List<IGameEvent>();
        }

        public void AddEvent(IGameEvent gameEvent)
        {
            if (gameEvent == null)
                throw new NullReferenceException($"Trying to add empty 'Game Event' in handler.");
            
            _events.Add(gameEvent);
        }
        
        public void AddEvents(IReadOnlyList<IGameEvent> gameEvents)
        {
            if (gameEvents == null)
                throw new NullReferenceException($"Trying to add empty 'Game Event' in handler.");
            
            _events.AddRange(gameEvents);
        }

        public void RemoveEvent(IGameEvent gameEvent) => _events.Remove(gameEvent);

        public void RemoveEvents(IReadOnlyList<IGameEvent> gameEvents)
        {
            foreach (var gameEvent in gameEvents) 
                RemoveEvent(gameEvent);
        }
    }
}