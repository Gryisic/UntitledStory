using System;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents.Bindings
{
    public class EventBinding<T> : IEventBinding<T> where T: IBusHandledEvent
    {
        public Action<T> Triggered { get; private set; } = _ => { };
        public Action TriggeredWithoutArgs { get; private set; } = () => { };
        
        public EventBinding(Action<T> callback) => Triggered = callback;
        public EventBinding(Action callback) => TriggeredWithoutArgs = callback;

        public void Add(Action<T> callback) => Triggered += callback;
        public void Add(Action callback) => TriggeredWithoutArgs += callback;
        
        public void Remove(Action<T> callback) => Triggered -= callback;
        public void Remove(Action callback) => TriggeredWithoutArgs -= callback;
    }
}