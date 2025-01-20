using Common.Dialogues.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Core.Data.Texts;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class DialogueStateArgs : GameStateArgs
    {
        public IDialogueDataProvider DialogueDataProvider { get; }
        public IDialogueEvent Event { get; }
        public NamesLocalization NamesLocalization { get; private set; }
        
        public DialogueStateArgs(IDialogueDataProvider dialogueDataProvider, IDialogueEvent gameEvent,
            Enums.GameStateFinalization finalization = Enums.GameStateFinalization.Full) : base(gameEvent, finalization)
        {
            DialogueDataProvider = dialogueDataProvider;
            Event = gameEvent;
        }

        public void SetLocalizationData(NamesLocalization localization) =>
            NamesLocalization = localization;
    }
}