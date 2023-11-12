using System.Collections.Generic;
using Common.Models.Triggers.Interfaces;
using UnityEngine;

namespace Common.Models.Triggers
{
    public class MonoTriggersHandler : MonoBehaviour
    {
        [SerializeField] private MonoTrigger[] _triggers;

        public IReadOnlyList<ITrigger> Triggers => _triggers;

        private void Awake()
        {
            if (_triggers.Length != transform.childCount)
            {
                Debug.LogWarning("Amount of triggers in 'Mono Triggers Handler' isn't equal amount of it child");

                _triggers = gameObject.GetComponentsInChildren<MonoTrigger>();
            }
        }
    }
}