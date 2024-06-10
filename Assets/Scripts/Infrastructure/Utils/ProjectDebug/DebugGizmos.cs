#if UNITY_EDITOR
using UnityEngine;

namespace Infrastructure.Utils.ProjectDebug
{
    public static class DebugGizmos
    {
        public static void DrawCell(Vector2 leftBottomPosition, Vector2 rightTopPosition, Color color)
        {
            Gizmos.color = color;
            
            Gizmos.DrawLine(leftBottomPosition, new Vector3(leftBottomPosition.x, rightTopPosition.y));
            Gizmos.DrawLine(leftBottomPosition, new Vector3(rightTopPosition.x, leftBottomPosition.y));
            Gizmos.DrawLine(rightTopPosition, new Vector3(leftBottomPosition.x, rightTopPosition.y));
            Gizmos.DrawLine(rightTopPosition, new Vector3(rightTopPosition.x, leftBottomPosition.y));
        }
    }
}
#endif