using System;
using UnityEngine;

namespace Common.Models.GameEvents.Bindings
{
    [Serializable]
    public abstract class BindingCallbackBase
    {
        [SerializeField, HideInInspector] private string _methodName;
        
        public abstract object Callback { get; }
        
        public abstract event Action<BindingCallbackBase> Fired;
    }
}