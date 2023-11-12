using Core.GameStates;

namespace Core.Interfaces
{
    public interface IGameStateSwitcher
    {
        void SwitchState<T>(GameStateArgs args) where T: IGameState;
    }
}