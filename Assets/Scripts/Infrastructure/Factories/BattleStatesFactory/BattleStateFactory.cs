using System.Collections.Generic;
using Common.Battle.Interfaces;
using Infrastructure.Factories.BattleStatesFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.BattleStatesFactory
{
    public class BattleStateFactory : IBattleStateFactory
    {
        private DiContainer _container;
        
        public IReadOnlyList<IBattleState> States { get; private set; }

        public BattleStateFactory(DiContainer container)
        {
            _container = container;
        }

        public void UpdateContainer(DiContainer container) => _container = container;

        public void CreateAllStates() => States = _container.ResolveAll<IBattleState>();
    }
}