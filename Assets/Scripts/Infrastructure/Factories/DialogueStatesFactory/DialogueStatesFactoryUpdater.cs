using System;
using Infrastructure.Factories.DialogueStatesFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.DialogueStatesFactory
{
    public class DialogueStatesFactoryUpdater : IInitializable, IDisposable
    {
        private readonly IDialogueStatesFactory _factory;

        private DiContainer _container;
        
        protected DialogueStatesFactoryUpdater([Inject(Source = InjectSources.Local)]DiContainer container, IDialogueStatesFactory factory)
        {
            _factory = factory;
            _container = container;
        }

        public void Initialize() => _factory.UpdateContainer(_container);

        public void Dispose() => _container = null;
    }
}