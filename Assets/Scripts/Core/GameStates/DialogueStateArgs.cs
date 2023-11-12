using Common.Dialogues.Interfaces;

namespace Core.GameStates
{
    public class DialogueStateArgs : GameStateArgs
    {
        public IDialogueDataProvider DialogueDataProvider { get; }
        
        public DialogueStateArgs(IDialogueDataProvider dialogueDataProvider)
        {
            DialogueDataProvider = dialogueDataProvider;
        }
    }
}