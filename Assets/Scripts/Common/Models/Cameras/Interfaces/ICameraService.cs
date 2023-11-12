using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Cameras.Interfaces
{
    public interface ICameraService : IService
    {
        UnityEngine.Camera SceneCamera { get; }
        
        void FollowUnit(Transform unitTransform, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);

        void FocusOn(Transform transformToFocusOn, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);
        void FocusOn(Vector2 positionToFocusOn, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);
        void SetEasingAndConfiner(Enums.CameraEasingType easingType, Collider2D confiner);

        void Shake();
    }
}