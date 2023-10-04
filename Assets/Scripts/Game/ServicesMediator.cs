using Game.Characters.Player;
using Game.Common;
using Game.Components.Transforms;
using Game.Data;
using Game.GameObjectsViews;
using Game.Loaders;
using Game.Locations;
using Game.Signals;

namespace Game
{
    public static class ServicesMediator
    {
        public static GameStarter GameStarter => Get<GameStarter>();
        public static SceneTransitionController SceneTransitionController => Get<SceneTransitionController>();
        public static GameObjectsPool GameObjectsPool => Get<GameObjectsPool>();
        public static GameObjectsController GameObjectsController => Get<GameObjectsController>();

        
        public static SignalsService Signals => Get<SignalsService>();
        public static TickableService TickableService => Get<TickableService>();
        public static IGameModelHolder GameModelHolder => Get<IGameModelHolder>();
        public static IDataController DataController => Get<IDataController>();
        public static GameDataLoader GameDataLoader => Get<GameDataLoader>();
        
        public static TransformsController TransformsController => Get<TransformsController>();
        public static LocationController LocationController => Get<LocationController>();
        public static PlayerCharacterController PlayerCharacterController => Get<PlayerCharacterController>();
        
        private static T Get<T>()
        {
            return Global.ServiceLocator.Get<T>();
        }
    }
}