using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class ExploringStateArgs : GameStateArgs
    {
        public Vector2 Position { get; }
        
        public ExploringStateArgs(Vector2 position, Enums.GameStateFinalization finalization = Enums.GameStateFinalization.Full, IGameEventData eventData = null) : base(eventData, finalization)
        {
            Position = position;
        }
        
        public ExploringStateArgs() : this(Vector2.zero) { }
    }
}