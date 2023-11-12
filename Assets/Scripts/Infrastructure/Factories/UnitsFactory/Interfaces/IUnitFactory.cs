using System.Collections.Generic;
using Common.Units;
using Common.Units.Templates;
using UnityEngine;

namespace Infrastructure.Factories.UnitsFactory.Interfaces
{
    public interface IUnitFactory
    {
        void Load(int id);
        void Load(IReadOnlyList<int> id);

        Unit Create(UnitTemplate template, Vector3 at);
    }
}