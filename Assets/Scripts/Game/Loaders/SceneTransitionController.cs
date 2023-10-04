using Common.Data;
using Common.ServiceLocator;
using Game.Data;
using Game.Locations;

namespace Game.Loaders
{
    public class SceneTransitionController : IInitializableService
    {
        private IDataController dataController;
        private SceneLoader sceneLoader;
        private SceneUnloader sceneUnloader;
        private LocationStarter locationStarter;
        private PreloaderController preloaderController;
        
        public void Initialize()
        {
            dataController = ServicesMediator.DataController;
            sceneLoader = Global.ServiceLocator.Get<SceneLoader>();
            sceneUnloader = Global.ServiceLocator.Get<SceneUnloader>();
            preloaderController = Global.ServiceLocator.Get<PreloaderController>();
            locationStarter = Global.ServiceLocator.Get<LocationStarter>();
        }

        public void Dispose()
        {
            
        }
        
        public async void Load(Id locationId)
        {
            dataController.SetCurrentLocation(locationId);

            await preloaderController.Show();

            await sceneUnloader.Unload();
            
            await sceneLoader.Load(locationId);

            await locationStarter.PrepareLocation();
            
            await preloaderController.Hide();
        }
    }
}