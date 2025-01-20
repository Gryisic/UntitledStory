using Common.Models.GameEvents.General;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.GameEvents.FieldObstacle
{
    public class CutObstacleEvent : FieldObstacleEvent
    {
        [SerializeField] private BoxCollider2D _collider;
        
        public override Enums.FieldSkill RequiredSkill => Enums.FieldSkill.Cut;
        
        public override void Execute()
        {
            _collider.gameObject.SetActive(false);
            
            End();
        }
    }
}