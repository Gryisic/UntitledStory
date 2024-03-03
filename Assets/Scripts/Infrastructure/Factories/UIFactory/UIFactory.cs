using Common.UI;
using Infrastructure.Factories.UIFactory.Interfaces;
using UnityEngine;

namespace Infrastructure.Factories.UIFactory
{
    public class UIFactory : IUIFactory
    {
        public T CreateCopy<T>(UIElement copyOf, Transform root) where T: UIElement
        {
            UIElement copy = GameObject.Instantiate(copyOf, root);

            return (T) copy;
        }
    }
}