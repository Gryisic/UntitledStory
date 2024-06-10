using UnityEngine;

namespace Common.Models.Triggers.Interfaces
{
    public interface IMonoTriggerData
    {
        Vector2 CollidedAt { get; }
    }
}