using Common.ServiceLocator;
using Zenject;

namespace Game.Common
{
    public class TickableService : IService
    {
        private readonly TickableManager tickableManager;

        public TickableService(TickableManager tickableManager)
        {
            this.tickableManager = tickableManager;
        }

        public void Add(ITickable tickable)
        {
            tickableManager.Add(tickable);
        }

        public void Remove(ITickable tickable)
        {
            tickableManager.Remove(tickable);
        }
    }
}