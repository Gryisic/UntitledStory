using Common.Dialogues.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Core.Data.Texts;

namespace Core.GameStates
{
    public class DialogueStateArgs : GameStateArgs
    {
        public IDialogueDataProvider DialogueDataProvider { get; }
        public IDialogueTrigger Trigger { get; }
        public NamesLocalization NamesLocalization { get; private set; }
        
        public DialogueStateArgs(IDialogueDataProvider dialogueDataProvider, IDialogueTrigger trigger) : base(trigger)
        {
            DialogueDataProvider = dialogueDataProvider;
            Trigger = trigger;
        }

        public void SetLocalizationData(NamesLocalization localization) =>
            NamesLocalization = localization;
    }
}