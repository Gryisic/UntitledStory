using System;
using Common.Models.GameEvents.Interfaces;
using Core.GameStates;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Common.Models.GameEvents.General
{
    public class DialogueEvent : GeneralEvent, IDialogueEvent
    {
        [Space, Header("Dialogue Data")]
        [SerializeField, FromParent("_id")] private string _key;
        
        public string Key => _key;

        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;

        public override void Execute() => 
            StateChangeRequested?.Invoke(Enums.GameStateType.Dialogue, new DialogueStateArgs(this, this));
    }
}