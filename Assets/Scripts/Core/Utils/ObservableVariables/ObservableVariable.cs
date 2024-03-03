using System;
using Core.Utils.ObservableVariables.Interfaces;

namespace Core.Utils.ObservableVariables
{
    public abstract class ObservableVariable<T> : IObservableVariable<T>
    {
        public abstract event Action<T> Changed;

        public abstract void Increase(T value);
        
        public abstract void Decrease(T value);

        public abstract void Reset();
    }
}