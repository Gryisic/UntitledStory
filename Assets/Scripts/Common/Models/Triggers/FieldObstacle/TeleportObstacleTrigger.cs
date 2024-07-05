using System;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.General;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers.FieldObstacle
{
    public class TeleportObstacleTrigger : FieldObstacleTrigger, IPositionChangeRequester
    {
        [SerializeField] private Transform _teleportTo;

        public override Enums.FieldSkill RequiredSkill => Enums.FieldSkill.Teleport;
        
        public event Action<Vector2> RequestPositionChange;
        public override event Action<IGameEvent> Ended;
        
        public override void Execute()
        {
            RequestPositionChange?.Invoke(_teleportTo.position);
            Ended?.Invoke(this);
        }
    }
}