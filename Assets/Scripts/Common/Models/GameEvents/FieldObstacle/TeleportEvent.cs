using System;
using Common.Models.GameEvents.General;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.GameEvents.FieldObstacle
{
    public class TeleportEvent : FieldObstacleEvent, IPositionChangeRequester
    {
        [SerializeField] private Transform _teleportTo;

        public override Enums.FieldSkill RequiredSkill => Enums.FieldSkill.Teleport;
        
        public event Action<Vector2> RequestPositionChange;
        
        public override void Execute()
        {
            RequestPositionChange?.Invoke(_teleportTo.position);
            
            End();
        }
    }
}