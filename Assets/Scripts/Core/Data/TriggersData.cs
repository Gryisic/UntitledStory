using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.Bindings;
using Common.Models.GameEvents.BusHandled;
using Common.Models.GameEvents.Interfaces;
using Core.Data.Interfaces;
using Core.Data.Triggers;
using UnityEngine;

namespace Core.Data
{
    [Serializable, CreateAssetMenu(menuName = "Core/Data/Triggers")]
    public class TriggersData : GameData, ITriggersData
    {
        [SerializeField] private List<EditorTrigger> _triggers;
        
        private List<string> _idList;

        private IEventBinding<TriggerActivatedEvent> _eventBinding;

        public bool IsDirty { get; private set; } = true;

        public void Initialize()
        {
            _idList = new List<string>();
            _eventBinding = new EventBinding<TriggerActivatedEvent>(Add);
            
            _idList.AddRange(_triggers.Where(t => t.IsActive).Select(t => t.ID));

            IsDirty = true;
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
        public IReadOnlyList<EditorTrigger> Triggers => _triggers;

        public void ActivateFromRedactor(string id) => _triggers.First(t => t.ID == id).Activate();
        
        public void DeactivateFromRedactor(string id) => _triggers.First(t => t.ID == id).Deactivate();
#endif
    }
}