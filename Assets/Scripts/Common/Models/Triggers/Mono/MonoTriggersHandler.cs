using System;
using System.Collections.Generic;
using Common.Models.Triggers.Extensions;
using Common.Models.Triggers.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Models.Triggers.Mono
{
    public class MonoTriggersHandler : MonoBehaviour, IDisposable
    {
        [SerializeField] private MonoTriggerZone[] _triggerZones;

        public IReadOnlyList<ITrigger> Triggers { get; private set; }
        public IReadOnlyList<IMonoTriggerZone> TriggerZones => _triggerZones;

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