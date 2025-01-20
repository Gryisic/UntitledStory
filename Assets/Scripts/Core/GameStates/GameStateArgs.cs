using System;
using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class GameStateArgs : EventArgs
    {
        public IGameEventData EventData { get; }
        public Enums.GameStateFinalization Finalization { get; }
        
        public GameStateArgs(IGameEventData eventData = null, Enums.GameStateFinalization finalization = Enums.GameStateFinalization.Full)
        {
            EventData = eventData;
            Finalization = finalization;
        }
    }
}