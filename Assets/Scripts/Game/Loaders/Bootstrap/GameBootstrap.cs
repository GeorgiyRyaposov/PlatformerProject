using System.Collections.Generic;
using Common.ServiceLocator;
using Game.Common;
using Game.Components;
using Game.Data;
using Game.GameObjectsViews;
using Game.Input;
using Game.Locations;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Loaders.Bootstrap
{
    public class GameBootstrap : MonoBehaviour, IInitializable
    {
        [SerializeField] private ScriptablesHolder componentsServicesHolder;
        [SerializeField] private ScriptablesHolder servicesHolder;
        [SerializeField] private MonoServiceLocator serviceLocator;

        [Inject] private SignalBus signalBus;
        [Inject] private TickableManager tickableManager;
        
        public void Initialize()
        {
            Global.ServiceLocator = serviceLocator;

            InputMediator.InputEventsHolder = new InputEventsHolder();
            
            RegisterServices();
            InitializeServices();
            
            DontDestroyOnLoad(gameObject);
        }

        private void InitializeServices()
        {
            var initializableServices = new List<IInitializableService>();
            serviceLocator.CollectAll(initializableServices);
            
            foreach (var service in initializableServices)
            {
                service.Initialize();
            }
        }

        private void RegisterServices()
        {
            serviceLocator.Register(new SignalsService(signalBus));
            serviceLocator.Register(new TickableService(tickableManager));
            serviceLocator.Register(new LocationStarter());
            serviceLocator.Register(new DataController());
            serviceLocator.Register(new GameDataLoader());
            serviceLocator.Register(new SceneTransitionController());
            serviceLocator.Register(new GameObjectsController());
            serviceLocator.Register(new InputDevicesManager());
            
            foreach (var scriptableService in servicesHolder.Values)
            {
                RegisterService(scriptableService);
            }

            foreach (var scriptableService in componentsServicesHolder.Values)
            {
                RegisterService(scriptableService);
            }
        }

        private void RegisterService(ScriptableObject scriptableService)
        {
            if (scriptableService is IService service)
            {
                serviceLocator.Register(service);
            }
            else
            {
                var scriptableName = scriptableService == null
                    ? "NULL"
                    : scriptableService.name;
                Debug.LogError($"<color=red>{scriptableName} is not service</color>");
            }
        }
    }
}