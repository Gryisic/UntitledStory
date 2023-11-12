using System.Collections.Generic;
using Core.Interfaces;

namespace Infrastructure.Factories.GameStatesFactory.Interfaces
{
    public interface IGameStatesFactory
    {
        IReadOnlyList<IGameState> GameModes { get; }
        
        void CreateAllStates();
    }
}