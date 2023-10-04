using System.Collections.Generic;
using Common.ServiceLocator;
using UnityEngine;

namespace Game.Loaders.Bootstrap
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private List<Component> services;
        [SerializeField] private MonoServiceLocator serviceLocator;
        
        private void Start()
        {
            RegisterServices();
            InitializeServices();
            
            Global.SceneServiceLocators.Add(serviceLocator);
        }

        private void OnDestroy()
        {
            Global.SceneServiceLocators.Remove(serviceLocator);
        }
        
        private void RegisterServices()
        {
            foreach (var monoBehaviour in services)
            {
                Register(monoBehaviour);
            }
        }

        private void Register(Component component)
        {
            if (component is IService service)
            {
                serviceLocator.Register(service);
            }
            else
            {
                var name = null == component ? "null" : component.name;
                Debug.LogError($"<color=red>{name} is not service</color>");
            }
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
    }
}