using Core.GameStates;

namespace Core.Interfaces
{
    public interface IGameState
    {
        void Activate(GameStateArgs args);
    }
}