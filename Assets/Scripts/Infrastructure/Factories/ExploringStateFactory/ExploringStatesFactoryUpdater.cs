using System;
using Infrastructure.Factories.ExploringStateFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.ExploringStateFactory
{
    public class ExploringStatesFactoryUpdater : IInitializable, IDisposable
    {
        private readonly IExploringStateFactory _factory;

        private DiContainer _container;
        
        protected ExploringStatesFactoryUpdater([Inject(Source = InjectSources.Local)]DiContainer container, IExploringStateFactory factory)
        {
            _factory = factory;
            _container = container;
        }

        public void Initialize() => _factory.UpdateContainer(_container);

        public void Dispose() => _container = null;
    }
}