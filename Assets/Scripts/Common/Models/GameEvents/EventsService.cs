using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.Interfaces;
using Core.Data.Interfaces;

namespace Common.Models.GameEvents
{
    public class EventsService : IEventsService
    {
        private readonly HashSet<IGameEvent> _events;
        private readonly IGameDataProvider _gameDataProvider;
        
        private IEventsData _eventsData;

        public IReadOnlyList<IGameEvent> Events => _events.ToList();

        public EventsService(IGameDataProvider gameDataProvider)
        {
            _events = new HashSet<IGameEvent>();
            _gameDataProvider = gameDataProvider;
        }

        public void Initialize()
        {
            _eventsData = _gameDataProvider.GetData<IEventsData>();
        }

        public void AddEvent(IGameEvent gameEvent)
        {
            if (gameEvent == null)
                throw new NullReferenceException($"Trying to add empty 'Game Event' in handler.");
            
            _events.Add(gameEvent);
        }
        
        public void AddEvents(IReadOnlyList<IGameEvent> gameEvents)
        {
            foreach (var gameEvent in gameEvents) 
                AddEvent(gameEvent);
        }

        public void RemoveEvent(IGameEvent gameEvent)
        {
            _events.Remove(gameEvent);
        }

        public void RemoveEvents(IReadOnlyList<IGameEvent> gameEvents)
        {
            foreach (var gameEvent in gameEvents) 
                RemoveEvent(gameEvent);
        }
    }
}