using System.Collections.Generic;
using UnityEngine;

namespace Common.Navigation.Interfaces
{
    public interface IBattleFieldSearchStrategy
    {
        bool TryFindField(IReadOnlyDictionary<Vector2, NavigationCell> cells, out BattleField field);
    }
}