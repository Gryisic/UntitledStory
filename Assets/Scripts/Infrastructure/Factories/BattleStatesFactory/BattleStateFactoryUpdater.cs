using System;
using Infrastructure.Factories.BattleStatesFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.BattleStatesFactory
{
    public class BattleStateFactoryUpdater : IInitializable, IDisposable
    {
        private readonly IBattleStateFactory _factory;

        private DiContainer _container;
        
        protected BattleStateFactoryUpdater([Inject(Source = InjectSources.Local)]DiContainer container, IBattleStateFactory factory)
        {
            _factory = factory;
            _container = container;
        }

        public void Initialize() => _factory.UpdateContainer(_container);

        public void Dispose() => _container = null;
    }
}