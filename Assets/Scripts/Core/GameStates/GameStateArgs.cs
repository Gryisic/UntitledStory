using System;
using Common.Models.GameEvents.Interfaces;

namespace Core.GameStates
{
    public class GameStateArgs : EventArgs
    {
        public IGameEventData EventData { get; }
        
        public GameStateArgs(IGameEventData eventData = null)
        {
            EventData = eventData;
        }
    }
}