using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Animator;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Templates
{
    public abstract class UnitTemplate : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private List<StandardAnimationData> _animationData;

        public int ID => _id;

        public CustomAnimation GetAnimation(Enums.StandardAnimation animation)
        {
            CustomAnimation finalAnimation = null;
            
            try
            {
                finalAnimation = _animationData.First(a => a.AnimationType == animation).Animation;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Animation {animation} of unit with id {_id} wasn't founded");
            }
            
            return finalAnimation;
        }

        [Serializable]
        private struct StandardAnimationData
        {
            [SerializeField] private Enums.StandardAnimation _animationType;
            [SerializeField] private CustomAnimation _animation;

            public Enums.StandardAnimation AnimationType => _animationType;
            public CustomAnimation Animation => _animation;
        }
    }
}