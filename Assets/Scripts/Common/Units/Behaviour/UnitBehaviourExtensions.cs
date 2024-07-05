using System;

namespace Common.Units.Behaviour
{
    public static class UnitBehaviourExtensions
    {
        public static void Dispose(this UnitBehaviour behaviour)
        {
            if (behaviour is IDisposable disposable)
                disposable.Dispose();
        }
    }
}