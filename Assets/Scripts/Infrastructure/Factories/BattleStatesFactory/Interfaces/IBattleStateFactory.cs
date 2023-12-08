using System.Collections.Generic;
using Common.Battle.Interfaces;
using Zenject;

namespace Infrastructure.Factories.BattleStatesFactory.Interfaces
{
    public interface IBattleStateFactory
    {
        IReadOnlyList<IBattleState> States { get; }

        void UpdateContainer(DiContainer container);
        void CreateAllStates();
    }
}