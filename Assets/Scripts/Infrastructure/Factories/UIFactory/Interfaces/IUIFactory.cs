using Common.UI;
using UnityEngine;

namespace Infrastructure.Factories.UIFactory.Interfaces
{
    public interface IUIFactory
    {
        T CreateCopy<T>(UIElement copyOf, Transform root) where T: UIElement;
    }
}