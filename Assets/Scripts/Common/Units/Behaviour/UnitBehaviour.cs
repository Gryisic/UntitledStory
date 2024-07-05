using System;
using Common.Units.Behaviour.Interfaces;

namespace Common.Units.Behaviour
{
    [Serializable]
    public abstract class UnitBehaviour
    {
        protected IBehaviourChanger behaviourChanger;

        public void Initialize(IBehaviourChanger changer) => behaviourChanger = changer;
        
        public abstract void Start();

        public abstract void End();
    }
}