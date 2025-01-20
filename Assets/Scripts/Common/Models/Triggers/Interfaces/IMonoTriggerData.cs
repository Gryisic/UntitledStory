using UnityEngine;

namespace Common.Models.Triggers.Interfaces
{
    public interface IMonoTriggerData
    {
        string SourceName { get; }
        Vector2 CollidedAt { get; }
    }
}