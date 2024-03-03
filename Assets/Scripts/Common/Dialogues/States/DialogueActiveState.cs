using System;
using System.Collections.Generic;
using System.Threading;
using Common.Dialogues.Interfaces;
using Common.Dialogues.Utils;
using Common.UI.Dialogue;
using Core.Data.Interfaces;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using Ink.Runtime;
using UnityEngine.InputSystem;

namespace Common.Dialogues.States
{
    public class DialogueActiveState : IDialogueState, IDeactivatable, IGameStateChangeRequester, IDisposable
    {
        private readonly IStateChanger<IDialogueState> _stateChanger;
        private readonly IInputService _inputService;
        private readonly ITriggersData _triggersData;
        
        private readonly Dialogue _dialogue;
        private readonly InkFunctionsResolver _functionsResolver;
        private readonly DialogueView _dialogueView;
        private readonly DialogueBoxView _boxView;
        private readonly DialogueChoicesView _choicesView;

        private CancellationTokenSource _dialogueTokenSource;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;

        public DialogueActiveState(IStateChanger<IDialogueState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, Dialogue dialogue, InkFunctionsResolver functionsResolver, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _inputService = servicesHandler.InputService;
            _triggersData = gameDataProvider.GetData<ITriggersData>();
            _dialogue = dialogue;
            _functionsResolver = functionsResolver;
            _dialogueView = ui.Get<DialogueView>();
            _boxView = _dialogueView.GetView<DialogueBoxView>();
            _choicesView = _dialogueView.GetView<DialogueChoicesView>();
        }
        
        public void Dispose()
        {
            _dialogueTokenSource?.Cancel();
            _dialogueTokenSource?.Dispose();
        }
        
        public void Activate()
        {
            SubscribeToEvents();
            AttachInput();

            _dialogueTokenSource = new CancellationTokenSource();
            
            ActivateDialogueAsync().Forget();
        }

        public void Deactivate()
        {
            DeAttachInput();
            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _dialogue.NamePrinted += _boxView.UpdateName;
            _dialogue.LetterPrinted += _boxView.UpdateSentence;
            _dialogue.Ended += OnDialogueEnded;
            _dialogue.ChoicesSelectionRequested += OnChoicesSelectionRequested;

            _choicesView.ChoiseTaken += OnChoiceTaken;
            
            _functionsResolver.ActivateTrigger += _triggersData.Add;
            _functionsResolver.DeactivateTrigger += _triggersData.Remove;
        }

        private void UnsubscribeToEvents()
        {
            _dialogue.NamePrinted -= _boxView.UpdateName;
            _dialogue.LetterPrinted -= _boxView.UpdateSentence;
            _dialogue.Ended -= OnDialogueEnded;
            _dialogue.ChoicesSelectionRequested -= OnChoicesSelectionRequested;

            _choicesView.ChoiseTaken -= OnChoiceTaken;

            _functionsResolver.ActivateTrigger -= _triggersData.Add;
            _functionsResolver.DeactivateTrigger -= _triggersData.Remove;
        }
        
        private void AttachInput()
        {
            _inputService.Input.Dialogue.NextSentence.performed += ToNextSentence;
            _inputService.Input.Dialogue.ToggleAutoMode.canceled += ToggleAutoDialogueMode;

            _inputService.Input.Dialogue.Enable();
        }

        private void DeAttachInput()
        {
            _inputService.Input.Dialogue.Disable();
            
            _inputService.Input.Dialogue.NextSentence.performed -= ToNextSentence;
            _inputService.Input.Dialogue.ToggleAutoMode.canceled -= ToggleAutoDialogueMode;
        }
        
        private async UniTask ActivateDialogueAsync()
        {
            await _dialogueView.ActivateAsync(_dialogueTokenSource.Token);
            
            _dialogue.Start(_dialogueTokenSource.Token);
        }
        
        private void OnDialogueEnded() => EndAsync().Forget();

        private void ToNextSentence(InputAction.CallbackContext context) => _dialogue.NextSentence();

        private void ToggleAutoDialogueMode(InputAction.CallbackContext context) => _dialogue.ToggleAutoMode();

        private void ToNextState() => RequestStateChange?.Invoke(Enums.GameStateType.Explore, new ExploringStateArgs());
        
        private void OnChoicesSelectionRequested(IReadOnlyList<Choice> choices)
        {
            _choicesView.SetIndexesAmount(choices.Count);

            foreach (var choice in choices) 
                _choicesView.SetNextChoiceText(choice.text);

            _choicesView.ActivateAsync(_dialogueTokenSource.Token).Forget();
        }
        
        private void OnChoiceTaken(int index)
        {
            _dialogue.SetChoice(index);
            
            _choicesView.DeactivateAsync(_dialogueTokenSource.Token).Forget();
        }
        
        private async UniTask EndAsync()
        {
            await _dialogueView.DeactivateAsync(_dialogueTokenSource.Token);
            
            ToNextState();
        }
    }
}