using System;

namespace Common.Models.Animator.Callbacks
{
    [Serializable]
    public abstract class AnimationCallback
    {
        public abstract void Execute();
    }
}