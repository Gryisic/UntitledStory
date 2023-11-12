using System;
using Core.Interfaces;

namespace Core.Extensions
{
    public static class GameModeExtensions
    {
        public static void Deactivate(this IGameState gameState)
        {
            if (gameState is IDeactivatable deactivatable)
                deactivatable.Deactivate();
        }
        
        public static void Dispose(this IGameState gameState)
        {
            if (gameState is IDisposable disposable)
                disposable.Dispose();
        }
        
        public static void Reset(this IGameState gameState)
        {
            if (gameState is IResettable resettable)
                resettable.Reset();
        }
    }
}