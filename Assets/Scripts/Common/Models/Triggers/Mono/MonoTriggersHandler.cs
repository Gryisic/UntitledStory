using System;
using System.Collections.Generic;
using Common.Models.Triggers.Extensions;
using Common.Models.Triggers.Interfaces;
using UnityEngine;

namespace Common.Models.Triggers.Mono
{
    public class MonoTriggersHandler : MonoBehaviour, IDisposable
    {
        [SerializeField] private MonoTrigger[] _triggers;

        public IReadOnlyList<IMonoTrigger> Triggers => _triggers;

        private void Awake()
        {
            if (_triggers.Length != transform.childCount)
            {
                Debug.LogWarning("Amount of triggers in 'Mono Triggers Handler' isn't equal amount of it child");

                _triggers = gameObject.GetComponentsInChildren<MonoTrigger>();
            }
        }

        public void Dispose()
        {
            foreach (var trigger in _triggers) 
                trigger.Dispose();
        }
    }
}