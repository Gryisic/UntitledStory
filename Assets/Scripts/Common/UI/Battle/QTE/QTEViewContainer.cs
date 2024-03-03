using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle.QTE
{
    public class QTEViewContainer : UIElement, IDisposable
    {
        [SerializeField] private Enums.QTEType _type;
        [SerializeField] private ConcreteQTEView _prefab;
        [SerializeField] private List<ConcreteQTEView> _views;

        public Enums.QTEType Type => _type;
        public ConcreteQTEView ViewForCopy => _prefab;
        public bool IsEmpty => _views.Count <= 0;
        
        public void Dispose()
        {
            _views.ForEach(v => v.Dispose());
        }
        
        public T Get<T>() where T: ConcreteQTEView
        {
            ConcreteQTEView view =_views.First();

            _views.Remove(view);
                
            return (T) view;
        }

        public void Return(ConcreteQTEView view) => _views.Add(view);
    }
}