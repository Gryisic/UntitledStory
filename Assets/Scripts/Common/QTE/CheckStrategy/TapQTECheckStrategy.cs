using System;
using Common.QTE.Interfaces;
using Common.QTE.Templates;
using Infrastructure.Utils;

namespace Common.QTE.CheckStrategy
{
    public class TapQTECheckStrategy : IQTEConditionCheckStrategy
    {
        private readonly QuickTimeEventTemplate _data;

        public event Action Succeeded;
        public event Action Failed;
        
        public TapQTECheckStrategy(QuickTimeEventTemplate data)
        {
            _data = data;
        }

        public void Start() { }

        public void Input(Enums.QTEState state, Enums.Input input)
        {
            if (state != Enums.QTEState.Opened || input != _data.Input || _data.Type != Enums.QTEType.Tap) 
                Failed?.Invoke();
            else
                Succeeded?.Invoke();
        }

        public void CancelInput(Enums.QTEState state) { }
    }
}