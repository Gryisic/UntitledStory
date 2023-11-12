using System.Collections.Generic;
using Core.Interfaces;
using Infrastructure.Factories.GameStatesFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.GameStatesFactory
{
    public class GameStatesFactory : IGameStatesFactory
    {
        private readonly DiContainer _container;
        
        public IReadOnlyList<IGameState> GameModes { get; private set; }

        public GameStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public void CreateAllStates() => GameModes = _container.ResolveAll<IGameState>();
    }
}