using System;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Core.GameStates;
using Infrastructure.Utils;

namespace Common.Models.Triggers.General
{
    public class DialogueTrigger : GeneralTrigger, IDialogueTrigger
    {
        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;
        public string Key => ID;

        public override event Action<IGameEvent> Ended;

        public override void Execute() => 
            StateChangeRequested?.Invoke(Enums.GameStateType.Dialogue, new DialogueStateArgs(this, this));

        public void End() => Ended?.Invoke(this);
    }
}