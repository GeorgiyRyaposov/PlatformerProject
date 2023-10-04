using System.Collections.Generic;

namespace Common.ServiceLocator
{
    public interface IServiceLocator
    {
        void Register<T>(T service) where T : IService;
        bool Unregister<T>(T service) where T : IService;
        T Get<T>();
        bool TryGet<T>(out T service);
        void CollectAll<T>(List<T> result);
    }
}