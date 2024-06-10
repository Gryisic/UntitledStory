using System;
using Common.UI.Extensions;
using Common.UI.Interfaces;
using Core.Data.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI
{
    public class UILayer : UIElement, IDisposable
    {
        [SerializeField] private Enums.UILayer _layer;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private UIElement[] _elements;
        
        public Enums.UILayer Layer => _layer;

        public void Initialize(IIconsData iconsData)
        {
            foreach (var element in _elements)
            {
                if (element is IIconsRequester requester)
                    requester.SetIconsData(iconsData);
            }
        }

        public void Dispose()
        {
            foreach (var element in _elements) 
                element.Dispose();
        }
        
        public void SetCamera(Camera sceneCamera) => _canvas.worldCamera = sceneCamera;

        public bool TryGetElement<T>(out T element) where T: UIElement
        {
            element = null;
            
            foreach (var uiElement in _elements)
            {
                if (uiElement is T concreteElement)
                {
                    element = concreteElement;

                    return true;
                }
            }

            return false;
        }
    }
}