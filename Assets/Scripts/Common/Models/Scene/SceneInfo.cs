using Common.Models.Triggers;
using UnityEngine;

namespace Common.Models.Scene
{
    public class SceneInfo : MonoBehaviour
    {
        [SerializeField] private Transform _unitsRoot;
        [SerializeField] private Transform _exploreUnitSpawnPoint;
        [SerializeField] private MonoTriggersHandler _monoTriggersHandler;
        
        public Transform UnitsRoot => _unitsRoot;
        public Transform ExploreUnitSpawnPoint => _exploreUnitSpawnPoint;
        public MonoTriggersHandler MonoTriggersHandler => _monoTriggersHandler;
    }
}