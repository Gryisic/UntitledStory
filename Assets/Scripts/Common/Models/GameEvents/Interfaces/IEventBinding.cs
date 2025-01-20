using System;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IEventBinding<in T> where T: IBusHandledEvent
    {
        Action<T> Triggered { get; }
        Action TriggeredWithoutArgs { get; }
    }
}