using System;

namespace Common.UI.Interfaces
{
    public interface IQueueableUIElement
    {
        event Action<UIElement> RequestAddingToQueue;
    }
}