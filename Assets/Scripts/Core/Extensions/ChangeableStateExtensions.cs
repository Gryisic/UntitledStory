using System;
using Core.Interfaces;

namespace Core.Extensions
{
    public static class ChangeableStateExtensions
    {
        public static void Deactivate(this IChangeableState gameState)
        {
            if (gameState is IDeactivatable deactivatable)
                deactivatable.Deactivate();
        }
        
        public static void Dispose(this IChangeableState gameState)
        {
            if (gameState is IDisposable disposable)
                disposable.Dispose();
        }
    }
}