using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.ServiceLocator
{
    public class MonoServiceLocator : MonoBehaviour, IServiceLocator
    {
        private readonly List<IService> services = new();

        public void Register<T>(T service) where T : IService
        {
            services.Add(service);
        }
        
        public bool Unregister<T>(T service) where T : IService
        {
            return services.Remove(service);
        }

        public T Get<T>()
        {
            for (var i = 0; i < services.Count; i++)
            {
                if (services[i] is T service)
                {
                    return service;
                }
            }

            return default;
        }
        
        public bool TryGet<T>(out T service)
        {
            for (var i = 0; i < services.Count; i++)
            {
                if (services[i] is T result)
                {
                    service = result;
                    return true;
                }
            }

            service = default;
            return false;
        }
        
        public void CollectAll<T>(List<T> result)
        {
            for (var i = 0; i < services.Count; i++)
            {
                if (services[i] is T service)
                {
                    result.Add(service);
                }
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < services.Count; i++)
            {
                if (services[i] is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            services.Clear();
        }
    }
}