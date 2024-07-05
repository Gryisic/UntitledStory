using System;
using Common.Models.Cameras;
using Common.Models.GameEvents;
using Common.Models.Scene;
using Common.Units.Handlers;
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
            BindUnitsHandlers();
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
            GeneralUnitsHandler generalUnitsHandler = Container.Resolve<GeneralUnitsHandler>();
            BattleUnitsHandler battleUnitsHandler = Container.Resolve<BattleUnitsHandler>();

            generalUnitsHandler.Dispose();
            battleUnitsHandler.Dispose();
            _sceneInfo.Dispose();
        }
        
        private void BindUnitsHandlers()
        {
            Container.Bind<UnitsPool>().AsSingle();
            Container.Bind<GeneralUnitsHandler>().AsSingle();
            Container.Bind<BattleUnitsHandler>().AsSingle();
        }

        private void BindSceneInfo() => Container.Bind<SceneInfo>().FromInstance(_sceneInfo).AsSingle();

        private void BindSelf() => Container.BindInterfacesAndSelfTo<SceneInstaller>().FromInstance(this).AsSingle();
    }
}