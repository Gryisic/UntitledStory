using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class GameStateArgs
    {
        public IGameEventData Data { get; }
        public Enums.GameStateFinalization Finalization { get; }
        
        protected GameStateArgs(IGameEventData data, Enums.GameStateFinalization finalization = Enums.GameStateFinalization.Full)
        {
            Finalization = finalization;
            Data = data;
        }

        public GameStateArgs()
        {
            
        }
    }
}