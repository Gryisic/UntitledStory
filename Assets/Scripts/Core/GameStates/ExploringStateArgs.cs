using Common.Models.GameEvents.Interfaces;
using UnityEngine;

namespace Core.GameStates
{
    public class ExploringStateArgs : GameStateArgs
    {
        public Vector2 Position { get; }
        
        public ExploringStateArgs(Vector2 position, IGameEventData eventData = null) : base(eventData)
        {
            Position = position;
        }
        
        public ExploringStateArgs() : this(Vector2.zero) { }
    }
}