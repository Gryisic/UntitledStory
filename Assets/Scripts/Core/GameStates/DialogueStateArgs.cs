using Common.Dialogues.Interfaces;
using Common.Models.GameEvents.Interfaces;

namespace Core.GameStates
{
    public class DialogueStateArgs : GameStateArgs
    {
        public IDialogueDataProvider DialogueDataProvider { get; }
        
        public DialogueStateArgs(IDialogueDataProvider dialogueDataProvider, IGameEventData eventData = null) : base(eventData)
        {
            DialogueDataProvider = dialogueDataProvider;
        }
    }
}