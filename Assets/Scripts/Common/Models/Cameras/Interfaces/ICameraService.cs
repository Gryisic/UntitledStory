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
        void SetEasingAndConfiner(Enums.CameraEasingType easingType, Collider2D confiner, float easingTime = Constants.DefaultCameraBlendTime);

        void Shake();

        Vector2 WorldToScreen(RectTransform rect);
        Vector2 WorldToScreen(Vector2 worldPosition, RectTransform rect);
        Vector2 ScreenToWorld(Enums.CameraCenterPositioning positioning = Enums.CameraCenterPositioning.Default);
        Vector2 ScreenToWorld(Vector2 screenPosition, Enums.CameraCenterPositioning positioning = Enums.CameraCenterPositioning.Default);
    }
}