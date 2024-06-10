using System;
using Common.Models.Triggers.Interfaces;
using Core.GameStates;
using Infrastructure.Utils;

namespace Common.Models.Triggers.General
{
    public class DialogueTrigger : GeneralTrigger, IDialogueTrigger
    {
        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;
        public string Key => ID;
        
        public override void Execute() => 
            StateChangeRequested?.Invoke(Enums.GameStateType.Dialogue, new DialogueStateArgs(this));
    }
}