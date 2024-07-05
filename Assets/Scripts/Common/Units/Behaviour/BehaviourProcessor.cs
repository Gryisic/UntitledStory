using System;
using System.Linq;
using Common.Units.Behaviour.Interfaces;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Units.Behaviour
{
    [Serializable]
    public class BehaviourProcessor : IBehaviourChanger, IDisposable
    {
        [SubclassesPicker, SerializeReference] private UnitBehaviour[] _exploringBehaviours;
        
        private UnitBehaviour _activeBehaviour;
        
        public void Initialize()
        {
            foreach (var behaviour in _exploringBehaviours) 
                behaviour.Initialize(this);
        }
        
        public void Dispose()
        {
            foreach (var behaviour in _exploringBehaviours) 
                behaviour.Dispose();
        }

        public void Start()
        {
            _activeBehaviour = _exploringBehaviours.First();
            _activeBehaviour.Start();
        }

        public void ForceToBehaviour<T>() where T: UnitBehaviour => ChangeBehaviour<T>();

        public void ChangeBehaviour<T>() where T : UnitBehaviour
        {
            _activeBehaviour?.End();

            _activeBehaviour = _exploringBehaviours.First(b => b is T);
            
            _activeBehaviour.Start();
        }
    }
}