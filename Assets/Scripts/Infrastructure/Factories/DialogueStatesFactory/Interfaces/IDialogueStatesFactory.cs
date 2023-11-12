using System.Collections.Generic;
using Common.Dialogues.Interfaces;
using Zenject;

namespace Infrastructure.Factories.DialogueStatesFactory.Interfaces
{
    public interface IDialogueStatesFactory
    {
        IReadOnlyList<IDialogueState> States { get; }

        void UpdateContainer(DiContainer container);
        void CreateAllStates();
    }
}