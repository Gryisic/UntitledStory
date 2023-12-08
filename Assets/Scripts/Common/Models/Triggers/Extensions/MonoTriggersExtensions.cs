using System;
using Common.Models.Triggers.Mono;

namespace Common.Models.Triggers.Extensions
{
    public static class MonoTriggersExtensions
    {
        public static void Dispose(this MonoTrigger trigger)
        {
            if (trigger is IDisposable disposable)
                disposable.Dispose();
        }
    }
}