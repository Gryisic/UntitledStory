using System;
using Common.QTE.Interfaces;
using Common.QTE.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.CheckStrategy
{
    public class MultiTapQTECheckStrategy : IQTEConditionCheckStrategy
    {
        private readonly QuickTimeEventTemplate _data;

        public event Action Succeeded;
        public event Action Failed;

        private int _tapCount;
        
        public MultiTapQTECheckStrategy(QuickTimeEventTemplate data)
        {
            _data = data;
        }

        public void Start()
        {
            MultiTapQTETemplate multiTapQteTemplate = _data as MultiTapQTETemplate;

            _tapCount = multiTapQteTemplate.TapCount;
        }

        public void Input(Enums.QTEState state, Enums.QTEInput input)
        {
            Debug.Log(state);
            
            if (state != Enums.QTEState.Opened || input != _data.Input || _data.Type != Enums.QTEType.MultiTap) 
                Failed?.Invoke();
            else
                CheckCount();
        }

        public void CancelInput(Enums.QTEState state) { }

        private void CheckCount()
        {
            _tapCount--;
            
            if (_tapCount <= 0)
                Succeeded?.Invoke();
        }
    }
}