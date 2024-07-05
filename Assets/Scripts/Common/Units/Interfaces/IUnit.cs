using System;
using Common.Units.Templates;

namespace Common.Units.Interfaces
{
    public interface IUnit : IUnitSharedData, IDisposable
    {
        void Initialize(UnitTemplate template);
        
        void Activate();
        void Deactivate();
    }
}