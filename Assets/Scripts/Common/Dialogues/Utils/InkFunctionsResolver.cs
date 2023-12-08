using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Utils;
using Ink.Runtime;
using UnityEngine;

namespace Common.Dialogues.Utils
{
    public class InkFunctionsResolver
    {
        private readonly IReadOnlyList<Enums.InkFunction> _functions;

        public event Action<string> ActivateTrigger;
        public event Action<string> DeactivateTrigger; 

        public InkFunctionsResolver()
        {
            _functions = Enum.GetValues(typeof(Enums.InkFunction)).Cast<Enums.InkFunction>().ToList();
        }
        
        public void Bind(Story story)
        {
            foreach (var function in _functions) 
                DefineCallback(story, function);
        }

        public void UnBind(Story story)
        {
            foreach (var function in _functions) 
                story.UnbindExternalFunction(GetFunctionName(function));
        }

        private void DefineCallback(Story story, Enums.InkFunction function)
        {
            switch (function)
            {
                case Enums.InkFunction.DebugMessage:
                    story.BindExternalFunction(GetFunctionName(function), (string message) => Debug.Log(message));
                    break;

                case Enums.InkFunction.ActivateTrigger:
                    story.BindExternalFunction(GetFunctionName(function), (string id) => ActivateTrigger?.Invoke(id));
                    break;
                
                case Enums.InkFunction.DeactivateTrigger:
                    story.BindExternalFunction(GetFunctionName(function), (string id) => DeactivateTrigger?.Invoke(id));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(function), function, null);
            }
        }

        private string GetFunctionName(Enums.InkFunction function) => Enum.GetName(typeof(Enums.InkFunction), function);
    }
}