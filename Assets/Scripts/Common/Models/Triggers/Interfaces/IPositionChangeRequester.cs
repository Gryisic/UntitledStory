using System;
using UnityEngine;

namespace Common.Models.Triggers.Interfaces
{
    public interface IPositionChangeRequester
    {
        event Action<Vector2> RequestPositionChange;
    }
}