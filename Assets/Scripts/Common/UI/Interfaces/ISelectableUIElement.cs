using System;

namespace Common.UI.Interfaces
{
    public interface ISelectableUIElement
    {
        event Action Selected;
        event Action Backed;
        
        void Select();
        void Back();
    }
}