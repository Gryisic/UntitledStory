using System;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.General;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers.FieldObstacle
{
    public class CutObstacleTrigger : FieldObstacleTrigger
    {
        [SerializeField] private BoxCollider2D _collider;
        
        public override Enums.FieldSkill RequiredSkill => Enums.FieldSkill.Cut;
        
        public override event Action<IGameEvent> Ended;
        
        public override void Execute()
        {
            _collider.gameObject.SetActive(false);

            Ended?.Invoke(this);
        }
    }
}