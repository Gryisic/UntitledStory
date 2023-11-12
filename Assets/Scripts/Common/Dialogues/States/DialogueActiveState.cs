using System;
using System.Threading;
using Common.Dialogues.Interfaces;
using Common.UI.Dialogue;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine.InputSystem;

namespace Common.Dialogues.States
{
    public class DialogueActiveState : IDialogueState, IDeactivatable, IGameStateChangeRequester, IDisposable
    {
        private readonly IStateChanger<IDialogueState> _stateChanger;
        private readonly IInputService _inputService;
        
        private readonly Dialogue _dialogue;
        private readonly DialogueView _dialogueView;

        private CancellationTokenSource _dialogueTokenSource;
        
        public event Action<Enums.GameStateType, GameStateArgs> RequestStateChange;

        public DialogueActiveState(IStateChanger<IDialogueState> stateChanger, IServicesHandler servicesHandler, Dialogue dialogue, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _inputService = servicesHandler.InputService;
            _dialogue = dialogue;
            _dialogueView = ui.Get<DialogueView>();
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
            _dialogue.NamePrinted += _dialogueView.UpdateName;
            _dialogue.LetterPrinted += _dialogueView.UpdateSentence;
            _dialogue.Ended += OnDialogueEnded;
        }

        private void UnsubscribeToEvents()
        {
            _dialogue.NamePrinted -= _dialogueView.UpdateName;
            _dialogue.LetterPrinted -= _dialogueView.UpdateSentence;
            _dialogue.Ended -= OnDialogueEnded;
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

        private void ToNextState() => RequestStateChange?.Invoke(Enums.GameStateType.Explore, new GameStateArgs());
        
        private async UniTask EndAsync()
        {
            await _dialogueView.DeactivateAsync(_dialogueTokenSource.Token);
            
            ToNextState();
        }
    }
}