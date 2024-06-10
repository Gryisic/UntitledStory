using System;
using System.Collections.Generic;
using System.Linq;
using Common.UI.Battle;
using Common.UI.Extensions;
using Core.Data.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using Zenject;

namespace Common.UI
{
    public class UI : MonoBehaviour, IDisposable
    {
        [SerializeField] private List<UILayer> _layers;

        private IIconsData _iconsData;
        
        public void Initialize(IGameDataProvider dataProvider)
        {
            _iconsData = dataProvider.GetData<IIconsData>();
            
            _layers.ForEach(l => l.Initialize(_iconsData));
        }
        
        public void Dispose()
        {
            foreach (var layer in _layers) 
                layer.Dispose();
        }
        
        public void SetCameraToLayer(Camera sceneCamera, Enums.UILayer layer)
        {
            UILayer uiLayer = _layers.First(l => l.Layer == layer);
            
            uiLayer.SetCamera(sceneCamera);
        }

        public T Get<T>() where T: UIElement
        {
            foreach (var layer in _layers)
            {
                if (layer.TryGetElement(out T element))
                    return element;
            }

            throw new InvalidOperationException($"Trying to get ui element that's not presented in ui layers. Element: {typeof(T)}");
        }
    }
}