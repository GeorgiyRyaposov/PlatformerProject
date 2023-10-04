using Game.Loaders.Bootstrap;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    [CreateAssetMenu(fileName = "BootstrapInstaller", menuName = "Installers/BootstrapInstaller")]
    public class BootstrapInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameBootstrap>().FromComponentInNewPrefab(gameBootstrap.gameObject).AsSingle();
        }
    }
}