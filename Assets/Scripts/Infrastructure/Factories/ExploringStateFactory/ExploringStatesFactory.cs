using System.Collections.Generic;
using Common.Exploring.Interfaces;
using Infrastructure.Factories.ExploringStateFactory.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories.ExploringStateFactory
{
    public class ExploringStatesFactory : IExploringStateFactory
    {
        private DiContainer _container;
        
        public IReadOnlyList<IExploringState> States { get; private set; }

        public ExploringStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public void UpdateContainer(DiContainer container) => _container = container;

        public void CreateAllStates() => States = _container.ResolveAll<IExploringState>();
    }
}