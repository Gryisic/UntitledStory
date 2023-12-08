using System.Collections.Generic;

namespace Common.Models.Animator.Interfaces
{
    public interface ICustomAnimation
    {
        IEnumerable<IAnimationFrameData> Frames { get; }
    }
}