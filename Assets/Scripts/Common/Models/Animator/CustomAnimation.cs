using System;
using System.Collections.Generic;
using Common.Models.Animator.Interfaces;
using UnityEngine;

namespace Common.Models.Animator
{
    [Serializable, CreateAssetMenu(menuName = "Common/Templates/Animations/Animation")]
    public class CustomAnimation : ScriptableObject, ICustomAnimation
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private List<AnimationFrame> _frames;

        public IReadOnlyList<IAnimationFrameData> Frames => _frames;

#if UNITY_EDITOR 
        //Some shit required for custom inspector
        public Sprite GetSprite(int index) => _sprites.Count <= index ? null : _sprites[index];

        public void UpdateSprites()
        {
            if (ReferenceEquals(_frames, null))
                return;
            
            for (var i = 0; i < _frames.Count; i++)
            {
                AnimationFrame frame = _frames[i];
                
                frame.SetSprite(_sprites[i]);
            }
        }

        public void AddSprite(int index, Sprite sprite) => AddFrame(index, sprite);

        public void AddFrame(int index, Sprite sprite = null)
        {
            _sprites.Insert(index, sprite);
            _frames.Insert(index, new AnimationFrame());
        }

        public void RemoveFrame(int index)
        {
            _sprites.RemoveAt(index);
            _frames.RemoveAt(index);
        }

        public float GetFrameHeight(int index) => _frames.Count <= index ? 0 : _frames[index].Height;
#endif
    }
}