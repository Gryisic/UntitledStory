using Cinemachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Cameras
{
    public abstract class Camera : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera virtualCamera;
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

        public void SetConfiner(Collider2D confiner) => confiner2D.m_BoundingShape2D = confiner;

        protected abstract float DistanceToSize(Enums.CameraDistanceType distanceType);
    }
}