using System;
using Common.Models.Triggers.Interfaces;
using Core.GameStates;
using Infrastructure.Utils;

namespace Common.Models.Triggers.Mono
{
    public class MonoDialogueTrigger : MonoTrigger, IDialogueTrigger
    {
        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;

        public string Key => firstTriggerInOrder.ID;

        public override void Execute()
        {
            base.Execute();
            
            StateChangeRequested?.Invoke(Enums.GameStateType.Dialogue, new DialogueStateArgs(this));
        }
    }
}