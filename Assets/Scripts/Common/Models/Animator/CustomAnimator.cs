using System;
using System.Collections.Generic;
using System.Threading;
using Common.Models.Animator.Callbacks;
using Common.Models.Animator.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Animator
{
    public class CustomAnimator : IDisposable
    {
        private readonly SpriteRenderer _renderer;
        private CancellationTokenSource _animationTokenSource;

        private ICustomAnimation _currentAnimation;

        public float CurrentAnimationDuration => _currentAnimation.Frames.Count / Constants.AnimationsFrameRate;
        
        public CustomAnimator(SpriteRenderer renderer)
        {
            _renderer = renderer;
        }

        public void Dispose()
        {
            _animationTokenSource?.Cancel();
            _animationTokenSource?.Dispose();
        }
        
        public void PlayOneShot(ICustomAnimation animation) => PlayAnimation(animation, true);

        public void PlayCyclic(ICustomAnimation animation)
        {
            if (animation == _currentAnimation)
                return;
            
            PlayAnimation(animation, false);
        }

        public void Stop() => _animationTokenSource?.Cancel();

        public void StopAtFirstFrame(ICustomAnimation animation)
        {
            Stop();

            if (animation != null)
                _renderer.sprite = animation.Frames[0].Sprite;
        }

        private void PlayAnimation(ICustomAnimation animation, bool isOneShot)
        {
            if (animation == null)
                return;
            
            _animationTokenSource?.Cancel();

            _animationTokenSource = new CancellationTokenSource();
            
            PlayAnimationAsync(animation, isOneShot).Forget();
        }

        private void HandleCallbacks(IEnumerable<AnimationCallback> callbacks)
        {
            foreach (var callback in callbacks)
            {
                callback.Execute();
            }
        }

        private async UniTask PlayAnimationAsync(ICustomAnimation animation, bool isOneShot)
        {
            _currentAnimation = animation;
            
            if (isOneShot)
            {
                await AwaitAnimationEndAsync(animation);
                
                return;
            }

            while (_animationTokenSource.IsCancellationRequested == false)
            {
                await AwaitAnimationEndAsync(animation);
            }
        }

        private async UniTask AwaitAnimationEndAsync(ICustomAnimation animation)
        {
            foreach (var frame in animation.Frames)
            {
                _renderer.sprite = frame.Sprite;
                
                if (frame.Callbacks.Count > 0)
                    HandleCallbacks(frame.Callbacks);
                
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.AnimationsFrameRate), cancellationToken: _animationTokenSource.Token);
            }
        }
    }
}