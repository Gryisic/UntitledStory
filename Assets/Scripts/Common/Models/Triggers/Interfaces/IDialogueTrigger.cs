using Common.Dialogues.Interfaces;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.Triggers.Interfaces
{
    public interface IDialogueTrigger : IGameStateChangerEvent, IDialogueDataProvider
    {
        
    }
}