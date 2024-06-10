using System;

namespace Common.Models.Triggers.Dependencies
{
    [Serializable]
    public abstract class Dependency
    {
        public abstract void Resolve();
    }
}