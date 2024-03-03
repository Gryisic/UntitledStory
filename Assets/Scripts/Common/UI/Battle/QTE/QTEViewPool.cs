using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factories.UIFactory;
using Infrastructure.Factories.UIFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle.QTE
{
    [Serializable]
    public class QTEViewPool : IDisposable
    {
        [SerializeField] private List<QTEViewContainer> _containers;

        private IUIFactory _uiFactory = new UIFactory();

        public void Dispose()
        {
            _containers.ForEach(c => c.Dispose());
        }
        
        public T Get<T>(Enums.QTEType qteType) where T: ConcreteQTEView
        {
            try
            {
                QTEViewContainer container = _containers.First(c => c.Type == qteType);

                T view = container.IsEmpty ? _uiFactory.CreateCopy<T>(container.ViewForCopy, container.Transform) : container.Get<T>();

                return view;
            }
            catch (Exception e)
            {
                Debug.LogError($"Container for {qteType} is not presented. Exception: {e}");
                throw;
            }
        }

        public void Return(Enums.QTEType qteType, ConcreteQTEView view)
        {
            try
            {
                QTEViewContainer container = _containers.First(c => c.Type == qteType);

                container.Return(view);
            }
            catch (Exception e)
            {
                Debug.LogError($"Container for {qteType} is not presented. Exception: {e}");
                throw;
            }
        }
    }
}