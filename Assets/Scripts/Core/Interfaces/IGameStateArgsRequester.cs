using System;
using Core.GameStates;

namespace Core.Interfaces
{
    public interface IGameStateArgsRequester<in T> where T: GameStateArgs
    {
        event Func<T> RequestArgs;
    }
}