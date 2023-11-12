using System;
using System.Collections.Generic;
using System.Linq;
using Common.UI.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI
{
    public class UI : MonoBehaviour, IDisposable
    {
        [SerializeField] private List<UILayer> _layers;

        public void SetCameraToLayer(Camera sceneCamera, Enums.UILayer layer)
        {
            UILayer uiLayer = _layers.First(l => l.Layer == layer);
            
            uiLayer.SetCamera(sceneCamera);
        }

        public void AddSceneUILayer(UILayer layer) => _layers.Add(layer);

        public void RemoveSceneUILayer(UILayer layer) => _layers.Remove(layer);
        
        public T Get<T>() where T: UIElement
        {
            foreach (var layer in _layers)
            {
                if (layer.TryGetElement(out T element))
                    return element;
            }

            throw new InvalidOperationException($"Trying to get ui element that's not presented in ui layers. Element: {typeof(T)}");
        }

        public void Dispose()
        {
            foreach (var layer in _layers) 
                layer.Dispose();
        }
    }
}