using Common.ServiceLocator;
using Game.Data.Scenes;
using Game.Signals;
using UnityEngine;

namespace Game.Locations
{
    [CreateAssetMenu(fileName = "LocationController", menuName = "Services/LocationController")]
    public class LocationController : ScriptableObject, IInitializableService
    {
        [SerializeField] private LocationsHolder locationsHolder;
        
        private LocationScenes currentLocation;
        private Bounds currentLocationBounds;
        
        public void Initialize()
        {
            ServicesMediator.Signals.Subscribe<LocationChanged>(OnLocationChanged);
        }

        public void Dispose()
        {
            ServicesMediator.Signals?.Unsubscribe<LocationChanged>(OnLocationChanged);
        }
        
        private void OnLocationChanged()
        {
            var currentLocationId = ServicesMediator.DataController.GetCurrentLocation();
            currentLocation = locationsHolder.Values.Find(x => x.Id == currentLocationId);
            currentLocationBounds = currentLocation.SceneBounds;
        }

        public bool IsPositionAtLocation(Vector3 position)
        {
            return currentLocationBounds.Contains(position);
        }

        public Vector3 GetSpawnPoint()
        {
            return currentLocation.PlayerSpawnPosition;
        }
    }
}