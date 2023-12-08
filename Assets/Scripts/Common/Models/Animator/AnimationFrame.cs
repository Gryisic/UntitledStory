using System;
using System.Collections.Generic;
using Common.Models.Animator.Callbacks;
using Common.Models.Animator.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Animator
{
    [Serializable]
    public class AnimationFrame : IAnimationFrameData
    {
        [SerializeField] private Sprite _sprite;
        [SerializeReference, SubclassesPicker] private AnimationCallback[] _callbacks;

        public Sprite Sprite => _sprite;
        public IReadOnlyList<AnimationCallback> Callbacks => _callbacks;

#if UNITY_EDITOR
        //Some shit required for custom inspector
        public bool Foldout { get; private set; }
        public int Index { get; private set; }
        public float Height { get; private set; }

        public void SetFoldoutAndIndex(bool foldout, int index)
        {
            Foldout = foldout;
            Index = index;
        }

        public void SetSprite(Sprite sprite) => _sprite = sprite;

        public void SetHeight(float height) => Height = height;
#endif
    }
}