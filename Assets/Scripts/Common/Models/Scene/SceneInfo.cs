using System;
using Common.Models.Triggers.Mono;
using UnityEngine;

namespace Common.Models.Scene
{
    public class SceneInfo : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _unitsRoot;
        [SerializeField] private Transform _exploreUnitSpawnPoint;
        [SerializeField] private MonoTriggersHandler _monoTriggersHandler;
        
        public Transform UnitsRoot => _unitsRoot;
        public Transform ExploreUnitSpawnPoint => _exploreUnitSpawnPoint;
        public MonoTriggersHandler MonoTriggersHandler => _monoTriggersHandler;

        public void Dispose()
        {
            _monoTriggersHandler.Dispose();
        }
    }
}