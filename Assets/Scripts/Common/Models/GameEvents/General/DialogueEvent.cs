using System;
using Common.Models.GameEvents.Interfaces;
using Core.GameStates;
using Infrastructure.Utils;

namespace Common.Models.GameEvents.General
{
    [Serializable]
    public class DialogueEvent : GeneralEvent, IDialogueEvent
    {
        public string Key => ID;

        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;

        public override void Execute() => 
            StateChangeRequested?.Invoke(Enums.GameStateType.Dialogue, new DialogueStateArgs(this, this));
    }
}