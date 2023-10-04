using System.Threading.Tasks;
using Common.ServiceLocator;
using UnityEngine;

namespace Game.Locations
{
    public class LocationStarter : IService
    {
        public async Task PrepareLocation()
        {
            var gameObjectsController = ServicesMediator.GameObjectsController;
            var transformsController = ServicesMediator.TransformsController;
            var locationController = ServicesMediator.LocationController;
            
            var gameObjects = gameObjectsController.GetGameObjects();
            var spawnedCount = 0;
            
            for (var i = 0; i < gameObjects.Count; i++)
            {
                var gameObjectId = gameObjects[i];
                var transformData = transformsController.GetTransformData(gameObjectId);
                if (!locationController.IsPositionAtLocation(transformData.Position))
                {
                    continue;
                }
                
                var view = gameObjectsController.CreateGameObjectView(gameObjectId);
                var transform = view.transform;
                
                transform.SetPositionAndRotation(transformData.Position, transformData.Rotation);
                transform.localScale = transformData.Scale;
                spawnedCount++;

                if (spawnedCount % 10 == 0)
                {
                    await Task.Yield();
                }
            }

            var spawnPoint = ServicesMediator.DataController.GetPlayerPosition();
            ServicesMediator.PlayerCharacterController.SpawnPlayer(spawnPoint);

            Debug.Log($"<color=green>Spawned items: {spawnedCount}</color>");
        }
    }
}