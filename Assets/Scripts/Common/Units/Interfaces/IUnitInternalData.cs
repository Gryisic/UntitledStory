using System;
using Common.Models.Animator;
using Common.Units.Templates;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitInternalData : IDisposable
    {
        UnitTemplate Data { get; }
        CustomAnimator Animator { get; }
        
        Transform Transform { get; }
        Rigidbody2D Rigidbody { get; }

        Vector2 MoveDirection { get; }

        void SetMoveDirection(Vector2 direction);
        void Flip(Vector2 direction);
    }
}