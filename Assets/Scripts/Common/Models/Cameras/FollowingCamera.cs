using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Cameras
{
    public class FollowingCamera : Camera
    {
        public void FollowUnit(Transform unitTransform, Enums.CameraDistanceType distanceType)
        {
            virtualCamera.Lens.OrthographicSize = DistanceToSize(distanceType);

            virtualCamera.Follow = unitTransform;
        }

        protected override float DistanceToSize(Enums.CameraDistanceType distanceType)
        {
            float size = distanceType switch
            {
                Enums.CameraDistanceType.Far => Constants.FarFollowCameraSize,
                Enums.CameraDistanceType.Close => Constants.CloseFollowCameraSize,
                _ => Constants.NeutralFollowCameraSize
            };

            return size;
        }
    }
}