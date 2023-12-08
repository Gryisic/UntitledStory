using System;
using Common.Dialogues.Interfaces;
using Common.Dialogues.States;
using Core.Extensions;
using Core.Interfaces;
using Infrastructure.Factories.DialogueStatesFactory.Interfaces;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class DialogueState : StatesChanger<IDialogueState>, IGameState, IDeactivatable, IResettable
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly IDialogueStatesFactory _dialogueStateFactory;

        private DialogueStateArgs _args;
        
        private bool _isInitialized;

        public DialogueState(IGameStateSwitcher stateSwitcher, IDialogueStatesFactory dialogueStateFactory)
        {
            _stateSwitcher = stateSwitcher;
            _dialogueStateFactory = dialogueStateFactory;
        }
        
        public void Activate(GameStateArgs args)
        {
            if (_isInitialized == false)
                Initialize();
            
            if (args is DialogueStateArgs dialogueStateArgs)
            {
                _args = dialogueStateArgs;

                SubscribeToEvents();
                
                ChangeState<DialogueInitializeState>();
            }
            else
            {
                throw new InvalidOperationException("Trying to initiate dialogue via non DialogueStateArgs");
            }
        }

        public void Deactivate()
        {
            currentState.Deactivate();
            
            UnsubscribeToEvents();
        }
        
        public void Reset()
        {
            _isInitialized = false;
        }

        private void Initialize()
        {
            _dialogueStateFactory.CreateAllStates();
            states = _dialogueStateFactory.States;

            _isInitialized = true;
        }

        private DialogueStateArgs GetArgs() => _args;

        private void SubscribeToEvents()
        {
            foreach (var state in states)
            {
                if (state is IGameStateChangeRequester stateChangeRequester)
                    stateChangeRequester.RequestStateChange += ToNextState;
                
                if (state is IDialogueArgsRequester argsRequester)
                    argsRequester.RequestArgs += GetArgs;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var state in states)
            {
                if (state is IGameStateChangeRequester stateChangeRequester)
                    stateChangeRequester.RequestStateChange -= ToNextState;
                
                if (state is IDialogueArgsRequester argsRequester)
                    argsRequester.RequestArgs -= GetArgs;
            }
        }
        
        private void ToNextState(Enums.GameStateType nextState, GameStateArgs args)
        {
            switch (nextState)
            {
                case Enums.GameStateType.SceneSwitch:
                    _stateSwitcher.SwitchState<SceneSwitchState>(args);
                    break;
                
                case Enums.GameStateType.Explore:
                    _stateSwitcher.SwitchState<ExploringState>(args);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
            }
        }
    }
}