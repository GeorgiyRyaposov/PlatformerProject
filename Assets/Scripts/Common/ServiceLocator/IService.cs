using System;

namespace Common.ServiceLocator
{
    public interface IService
    {
        
    }

    public interface IInitializableService : IService, IDisposable
    {
        void Initialize();
    }
}