using Common.Dialogues.Interfaces;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IDialogueEvent : IGameStateChangerEvent, IDialogueDataProvider
    {
        
    }
}