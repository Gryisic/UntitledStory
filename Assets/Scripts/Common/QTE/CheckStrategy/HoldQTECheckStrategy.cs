using System;
using Common.QTE.Interfaces;
using Common.QTE.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.CheckStrategy
{
    public class HoldQTECheckStrategy : IQTEConditionCheckStrategy
    {
        private readonly QuickTimeEventTemplate _data;

        private float _startedAt;
        private float _holdTimer;

        public event Action Succeeded;
        public event Action Failed;
        
        public HoldQTECheckStrategy(QuickTimeEventTemplate data)
        {
            _data = data;
        }

        public void Start() => _startedAt = Time.fixedTime;

        public void Input(Enums.QTEState state, Enums.QTEInput input)
        {
            if (state != Enums.QTEState.Opened || input != _data.Input || _data.Type != Enums.QTEType.Hold 
                || Math.Abs(_startedAt + _data.OpenDelay - Time.fixedTime) > Constants.QTEButtonHoldThreshold)
            {
                Failed?.Invoke();
                
                return;
            }

            _holdTimer = Time.fixedTime;
        }

        public void CancelInput(Enums.QTEState state)
        {
            if (state != Enums.QTEState.Opened)
                return;

            float holdTime = Time.fixedTime - _holdTimer;
            
            if (holdTime < Constants.QTEButtonHoldThreshold)
            {
                Failed?.Invoke();
                
                return;
            }

            HoldQTETemplate holdData = _data as HoldQTETemplate;

            if (Math.Abs(holdTime - holdData.HoldDuration) < Constants.QTEButtonHoldThreshold) 
                Succeeded?.Invoke();
            else
                Failed?.Invoke();
        }
    }
}