using System;

namespace Core.Utils.ObservableVariables.Interfaces
{
    public interface IObservableVariable<out T>
    {
        event Action<T> Changed;
    }
}