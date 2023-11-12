using System;
using Cinemachine;
using Common.Models.Cameras.Interfaces;
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

        public void SetEasingAndConfiner(Enums.CameraEasingType easingType, Collider2D confiner)
        {
            _brain.m_DefaultBlend = DefineBlend(easingType);
            
            _focusCamera.SetConfiner(confiner);
            _followingCamera.SetConfiner(confiner);
        }

        public void Shake()
        {
            _impulseSource.GenerateImpulse();
        }

        private void ChangeCamera(Camera newCamera)
        {
            if (_activeCamera != null) 
                _activeCamera.Deactivate();
            
            _activeCamera = newCamera;
            _activeCamera.Activate();
        }
        
        private CinemachineBlendDefinition DefineBlend(Enums.CameraEasingType easingType)
        {
            switch (easingType)
            {
                case Enums.CameraEasingType.Instant:
                    return new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, Constants.DefaultCameraBlendTime);
                
                case Enums.CameraEasingType.Smooth:
                    return new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, Constants.DefaultCameraBlendTime);

                default:
                    throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null);
            }
        }
    }
}