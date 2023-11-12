using System;
using Common.Models.Cameras;
using Common.Models.Scene;
using Common.Units;
using Core.Interfaces;
using Core.Utils;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class SceneInstaller : MonoInstaller, IInitializable, IDisposable
    {
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private SceneInfo _sceneInfo;

        public override void InstallBindings()
        {
            BindUnitsHandler();
            BindSceneInfo();
            BindSelf();
        }

        public void Initialize()
        {
            ServicesHandler handler = (ServicesHandler) Container.Resolve<IServicesHandler>();
            
            handler.RegisterService(_cameraService);
        }
        
        public void Dispose()
        {
            UnitsHandler unitsHandler = Container.Resolve<UnitsHandler>();
            
            unitsHandler.Dispose();
        }
        
        private void BindUnitsHandler() => Container.Bind<UnitsHandler>().AsSingle();

        private void BindSceneInfo() => Container.Bind<SceneInfo>().FromInstance(_sceneInfo).AsSingle();

        private void BindSelf() => Container.BindInterfacesAndSelfTo<SceneInstaller>().FromInstance(this).AsSingle();
    }
}