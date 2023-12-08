using System.Collections.Generic;
using UnityEngine;

namespace Common.Navigation.Interfaces
{
    public interface IBattleFieldPlacementStrategy
    {
        BattleField GetField(IReadOnlyDictionary<Vector2, NavigationCell> cells);
    }
}