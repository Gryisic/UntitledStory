using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.BusHandled;
using Core.Data.Events;
using Core.Data.Interfaces;
using UnityEngine;

namespace Core.Data
{
    [Serializable, CreateAssetMenu(menuName = "Core/Data/Events")]
    public class EventsData : GameData, IEventsData, IDisposable
    {
        [SerializeField] private List<EventData> _events;
        
        private List<string> _idList;
        
        public bool IsDirty { get; private set; } = true;
        
        public void Initialize()
        {
            _idList = new List<string>();
            
            _idList.AddRange(_events.Where(t => t.IsActive).Select(t => t.ID));

            IsDirty = true;
        }
        
        public void Dispose()
        {
            _events.ForEach(e => e.Dispose());
        }

        public void Add(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("Trying to add null or empty id in 'TriggersData'");
            
            IsDirty = true;

            _idList.Add(id);
        }

        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("Trying to remove null or empty id in 'TriggersData'");
            
            IsDirty = true;
            
            _idList.Remove(id);
        }

        public IReadOnlyList<string> GetIDList()
        {
            IsDirty = false;

            return _idList;
        }

        private void Add(TriggerActivatedEvent @event)
        {
            Debug.Log($"Added {@event.ID}");
            Add(@event.ID);
        }

#if UNITY_EDITOR
        public static string EventsPropertyName => nameof(_events);
        
        public IReadOnlyList<EventData> Events => _events;

        public void ActivateFromRedactor(string id) => _events.First(t => t.ID == id).Activate();
        
        public void DeactivateFromRedactor(string id) => _events.First(t => t.ID == id).Deactivate();
#endif
    }
}