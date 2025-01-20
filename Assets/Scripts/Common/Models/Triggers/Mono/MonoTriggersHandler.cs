using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Extensions;
using Common.Models.Triggers.Interfaces;
using UnityEngine;

namespace Common.Models.Triggers.Mono
{
    public class MonoTriggersHandler : MonoBehaviour, IDisposable
    {
        [SerializeField] private MonoTriggerZone[] _triggerZones;

        public IReadOnlyList<ITrigger> Triggers { get; private set; }
        public IReadOnlyList<IMonoTriggerZone> TriggerZones => _triggerZones;
        public IReadOnlyList<IGameEvent> Events => Triggers.Select(t => t.Event).ToList();

        private void Awake()
        {
            if (_triggerZones.Length != transform.childCount)
            {
                Debug.LogWarning("Amount of triggers in 'Mono Triggers Handler' isn't equal amount of it child");

                _triggerZones = gameObject.GetComponentsInChildren<MonoTriggerZone>();
            }

            UpdateTriggersList();
        }

        public void Dispose()
        {
            foreach (var trigger in _triggerZones) 
                trigger.Dispose();
        }

        private void UpdateTriggersList()
        {
            List<ITrigger> triggers = new List<ITrigger>();

            foreach (var zone in _triggerZones)
                triggers.AddRange(zone.Triggers);

            Triggers = triggers;
        }
    }
}