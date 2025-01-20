using System;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents.Bindings
{
    [Serializable]
    public abstract class BindingCallback<T> : BindingCallbackBase where T: IBusHandledEvent
    {
        
    }
}