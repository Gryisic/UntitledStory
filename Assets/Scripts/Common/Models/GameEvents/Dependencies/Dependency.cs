using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.GameEvents.Dependencies
{
    [Serializable]
    public abstract class Dependency
    {
        [SerializeField] private Enums.AfterEventBehaviour _afterEventBehaviour;

        public Enums.AfterEventBehaviour AfterEventBehaviour => _afterEventBehaviour;
        
        public abstract void Resolve();
    }
}