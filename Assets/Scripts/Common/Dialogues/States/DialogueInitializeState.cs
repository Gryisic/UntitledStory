using System;
using Common.Dialogues.Interfaces;
using Core.Data.Interfaces;
using Core.Data.Texts;
using Core.GameStates;
using Core.Interfaces;
using Ink.Runtime;

namespace Common.Dialogues.States
{
    public class DialogueInitializeState : IDialogueState, IDialogueArgsRequester
    {
        private readonly IStateChanger<IDialogueState> _stateChanger;
        private readonly IGameDataProvider _gameDataProvider;
        private readonly ITextsData _textsData;
        
        private readonly Dialogue _dialogue;
        public event Func<DialogueStateArgs> RequestArgs;

        public DialogueInitializeState(IStateChanger<IDialogueState> stateChanger, IGameDataProvider gameDataProvider, Dialogue dialogue)
        {
            _stateChanger = stateChanger;
            _gameDataProvider = gameDataProvider;
            _textsData = _gameDataProvider.GetData<ITextsData>();

            _dialogue = dialogue;
        }

        public void Activate()
        {
            DialogueStateArgs args = RequestArgs?.Invoke();
            NamesLocalization localization = _textsData.GetLocalizedData<NamesLocalization>();
            string jsonStory = _textsData.GetTextAsset(args.DialogueDataProvider.Key).text;
            Story story = new Story(jsonStory);
            
            args.SetLocalizationData(localization);
            _dialogue.SetStory(story);
            
            _stateChanger.ChangeState<DialogueActiveState>();
        }
    }
}