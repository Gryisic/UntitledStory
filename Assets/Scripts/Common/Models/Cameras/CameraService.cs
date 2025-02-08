using System;
using Unity.Cinemachine;
using Common.Models.Cameras.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Cameras
{
    public class CameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] private CinemachineBrain _brain;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        [SerializeField] private FollowingCamera _followingCamera;
        [SerializeField] private FocusCamera _focusCamera;

        private Camera _activeCamera;

        public UnityEngine.Camera SceneCamera => _brain.OutputCamera;

        public void FollowUnit(Transform unitTransform, Enums.CameraDistanceType distanceType = Enums.CameraDistanceType.Neutral)
        {
            ChangeCamera(_followingCamera);

            _followingCamera.FollowUnit(unitTransform, distanceType);
        }

        public void FocusOn(Transform transformToFocusOn, Enums.CameraDistanceType distanceType = Enums.CameraDistanceType.Neutral) => 
            FocusOn(transformToFocusOn.position, distanceType);

        public void FocusOn(Vector2 positionToFocusOn, Enums.CameraDistanceType distanceType = Enums.CameraDistanceType.Neutral)
        {
            ChangeCamera(_focusCamera);
            
            _focusCamera.FocusOn(positionToFocusOn, distanceType);
        }

        public void SetEasingAndConfiner(Enums.CameraEasingType easingType, Collider2D confiner, float easingTime = Constants.DefaultCameraBlendTime)
        {
            _brain.DefaultBlend = DefineBlend(easingType, easingTime);
            
            _focusCamera.SetConfiner(confiner);
            _followingCamera.SetConfiner(confiner);
        }

        public void Shake()
        {
            _impulseSource.GenerateImpulse();
        }

        public Vector2 WorldToScreen(RectTransform rect) 
            => WorldToScreen(_activeCamera.transform.position, rect);

        public Vector2 WorldToScreen(Vector2 worldPosition, RectTransform rect)
        {
            Vector3 screenPoint = _brain.OutputCamera.WorldToScreenPoint(worldPosition);
            screenPoint.z = 0;

            return screenPoint;
        }

        public Vector2 ScreenToWorld(Enums.CameraCenterPositioning positioning = Enums.CameraCenterPositioning.Default) 
            => ScreenToWorld(_activeCamera.transform.position, positioning);

        public Vector2 ScreenToWorld(Vector2 screenPosition, Enums.CameraCenterPositioning positioning = Enums.CameraCenterPositioning.Default)
        {
            Vector2 defaultPosition = _brain.OutputCamera.ScreenToWorldPoint(screenPosition);
            
            switch (positioning)
            {
                case Enums.CameraCenterPositioning.Default:
                    return defaultPosition;

                case Enums.CameraCenterPositioning.Center:
                    return _brain.OutputCamera.GetScreenCenter();
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(positioning), positioning, null);
            }
        }

        private void ChangeCamera(Camera newCamera)
        {
            if (_activeCamera != null) 
                _activeCamera.Deactivate();
            
            _activeCamera = newCamera;
            _activeCamera.Activate();
        }

        private CinemachineBlendDefinition DefineBlend(Enums.CameraEasingType easingType, float blendTime)
        {
            switch (easingType)
            {
                case Enums.CameraEasingType.Instant:
                    return new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, blendTime);
                
                case Enums.CameraEasingType.Smooth:
                    return new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, blendTime);

                default:
                    throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null);
            }
        }
    }
}