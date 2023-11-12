using System.Collections.Generic;
using Common.Dialogues.Interfaces;
using Infrastructure.Factories.DialogueStatesFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.DialogueStatesFactory
{
    public class DialogueStatesFactory : IDialogueStatesFactory
    {
        private DiContainer _container;
        
        public IReadOnlyList<IDialogueState> States { get; private set; }

        public DialogueStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public void UpdateContainer(DiContainer container) => _container = container;

        public void CreateAllStates() => States = _container.ResolveAll<IDialogueState>();
    }
}