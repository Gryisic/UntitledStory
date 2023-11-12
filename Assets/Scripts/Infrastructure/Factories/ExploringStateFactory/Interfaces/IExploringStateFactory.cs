using System.Collections.Generic;
using Common.Exploring.Interfaces;
using Zenject;

namespace Infrastructure.Factories.ExploringStateFactory.Interfaces
{
    public interface IExploringStateFactory 
    {
        IReadOnlyList<IExploringState> States { get; }

        void UpdateContainer(DiContainer container);
        void CreateAllStates();
    }
}