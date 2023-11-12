using System;
using Common.Models.Triggers.Interfaces;
using Core.GameStates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers
{
    public class MonoDialogueTrigger : MonoTrigger, IDialogueTrigger
    {
        [SerializeField] private string _key;
        
        public override event Action Triggered;
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;

        public string Key => _key;

        protected override void Awake()
        {
            if (_key == string.Empty)
                throw new ArgumentNullException($"Key of dialogue trigger is empty. Name {name} ID {GetInstanceID()}");
            
            base.Awake();
        }

        public override void Execute()
        {
            base.Execute();
            
            RequestStateChange?.Invoke(Enums.GameStateType.Dialogue, new DialogueStateArgs(this));
        }
    }
}