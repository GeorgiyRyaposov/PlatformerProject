using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    [CreateAssetMenu(fileName = "SignalsInstaller", menuName = "Installers/SignalsInstaller")]
    public class SignalsInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<LocationChanged>();
        }
    }
}