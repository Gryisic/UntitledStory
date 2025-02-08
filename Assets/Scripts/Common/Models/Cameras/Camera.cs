using Unity.Cinemachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Cameras
{
    public abstract class Camera : MonoBehaviour
    {
        [SerializeField] protected CinemachineCamera virtualCamera;
        [SerializeField] protected CinemachineImpulseListener impulseListener;
        [SerializeField] private CinemachineConfiner2D confiner2D;
        
        public void Activate()
        {
            virtualCamera.Priority = Constants.ActivatedCameraPriority;

            impulseListener.enabled = true;
        }

        public void Deactivate()
        {
            impulseListener.enabled = false;
            
            virtualCamera.Follow = null;
            virtualCamera.Priority = Constants.DeactivatedCameraPriority;
        }

        public void SetConfiner(Collider2D confiner) => confiner2D.BoundingShape2D = confiner;

        protected abstract float DistanceToSize(Enums.CameraDistanceType distanceType);
    }
}