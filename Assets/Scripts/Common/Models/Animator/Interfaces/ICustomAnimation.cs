using System.Collections.Generic;

namespace Common.Models.Animator.Interfaces
{
    public interface ICustomAnimation
    {
        IReadOnlyList<IAnimationFrameData> Frames { get; }
    }
}