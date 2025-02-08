using System;
using System.Reflection;
using Common.Models.GameEvents.Bindings;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents.Bus
{
    public static class EventBusUtils
    {
        public static object GetAndRegisterGenericBinding(Type bindingType, object callback)
        {
            ValidateCallback(callback);
            
            object[] parameters = { bindingType, callback };
            MethodInfo methodInfo = typeof(EventBusUtils).GetMethod(nameof(AddToBus), BindingFlags.NonPublic | BindingFlags.Static);
            
            return methodInfo.MakeGenericMethod(bindingType).Invoke(null, parameters);
        }

        public static bool UnregisterGenericBinding(object binding)
        {
            Type bindingType;
            
            try
            {
                bindingType = GetBindingType(binding);
            }
            catch (NullReferenceException e)
            {
                return false;
            }
            MethodInfo methodInfo = typeof(EventBusUtils).GetMethod(nameof(RemoveFromBus), BindingFlags.NonPublic | BindingFlags.Static);
            
            return (bool) methodInfo.MakeGenericMethod(bindingType).Invoke(null, new []{ binding });
        }
        
        private static object AddToBus<T>(Type type, object callback) where T: IBusHandledEvent
        {
            EventBinding<T> binding = GetBinding<T>(type, callback);
            EventBus<T>.Add(binding);
            return binding;
        }

        private static bool RemoveFromBus<T>(object binding) where T: IBusHandledEvent 
            => EventBus<T>.Remove(binding as IEventBinding<T>);

        private static EventBinding<T> GetBinding<T>(Type type, object callback) where T: IBusHandledEvent
        {
            Type binding = typeof(EventBinding<>);
            Type constructedClass = binding.MakeGenericType(type);
            
            return Activator.CreateInstance(constructedClass, args: callback) as EventBinding<T>;
        }

        private static void ValidateCallback(object callback)
        {
            if (callback == null)
                return;
            
            Type callbackType = callback.GetType();

            if (callbackType.IsGenericType == false)
                return;

            Type[] arguments = callbackType.GetGenericArguments();

            if (arguments.Length > 1)
                throw new ArgumentException($"Callback of type {callbackType} has invalid amount of arguments(more then 1).");
            
            if (typeof(IBusHandledEvent).IsAssignableFrom(arguments[0]) == false)
                throw new ArgumentException($"Callback of type {callbackType} has invalid argument(not assigned from '{nameof(IBusHandledEvent)}'). Argument: {arguments[0]}");
        }

        private static Type GetBindingType(object binding)
        {
            Type type = binding.GetType();
            
            if (type.IsGenericType == false || Array.Exists(type.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventBinding<>)) == false)
                throw new ArgumentException($"Type of binding {binding} is invalid. Type: {binding.GetType()}");

            return type.GetGenericArguments()[0];
        }
    }
}