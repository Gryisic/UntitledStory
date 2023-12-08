using System.Collections.Generic;
using Common.Models.Animator.Callbacks;
using UnityEngine;

namespace Common.Models.Animator.Interfaces
{
    public interface IAnimationFrameData
    {
        Sprite Sprite { get; }
        IReadOnlyList<AnimationCallback> Callbacks { get; }
    }
}