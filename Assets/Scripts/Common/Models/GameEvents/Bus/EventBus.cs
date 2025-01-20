using System.Collections.Generic;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents.Bus
{
    public static class EventBus<T> where T: IBusHandledEvent
    {
        private static readonly HashSet<IEventBinding<T>> Bindings = new();

        public static void Add(IEventBinding<T> binding) => Bindings.Add(binding);
        public static bool Remove(IEventBinding<T> binding) => Bindings.Remove(binding);

        public static void Invoke(T @event)
        {
            foreach (var binding in Bindings)
            {
                binding.Triggered.Invoke(@event);
                binding.TriggeredWithoutArgs.Invoke();
            }
        }

        private static void Clear() => Bindings.Clear();
    }
}