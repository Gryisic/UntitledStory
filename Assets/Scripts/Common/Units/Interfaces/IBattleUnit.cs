using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IBattleUnit : IUnit, IBattleUnitSharedData
    {
        void MoveToPoint(Vector3 point);
        void MoveToPointAndLookAt(Vector3 point, Vector2 lookDirection);
        void ExecuteActions();
        void Restore();

        UniTask ExecuteActionsWithAwaiter();
    }
}