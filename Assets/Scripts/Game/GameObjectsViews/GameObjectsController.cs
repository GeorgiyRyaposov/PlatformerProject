using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.GameObjects;
using Common.ServiceLocator;
using Game.Data;
using Game.Loaders;
using UnityEngine;

namespace Game.GameObjectsViews
{
    public class GameObjectsController : IInitializableService, ICleanUpOnLocationUnload
    {
        public int SpawnedViewsCount => spawnedViews.Count;
        
        private GameObjectsPool gameObjectsPool;
        private IDataController dataController;
        
        private readonly List<GameObjectView> spawnedViews = new();
        
        public void Initialize()
        {
            gameObjectsPool = ServicesMediator.GameObjectsPool;
            dataController = ServicesMediator.DataController;
        }
        public void Dispose()
        {
            CleanUp();
        }

        
        public GameObjectView CreateGameObjectView(Id instanceId)
        {
            var templateId = GetTemplateId(instanceId);
            var view = gameObjectsPool.Get(templateId);
            view.InstanceId = instanceId;

            spawnedViews.Add(view);
            
            return view;
        }

        public GameObjectView GetGameObjectView(Id instanceId)
        {
            return spawnedViews.Find(x => x.InstanceId == instanceId);
        }

        public List<Id> GetGameObjects()
        {
            return dataController.GetGameObjectsToTemplatesMap().Keys.ToList();
        }
        
        public void CollectSpawnedViews(List<GameObjectView> views)
        {
            views.AddRange(spawnedViews);
        }

        private Id GetTemplateId(Id instanceId)
        {
            var map = dataController.GetGameObjectsToTemplatesMap();
            if (map.TryGetValue(instanceId, out var templateId))
            {
                return templateId;
            }

            Debug.LogError($"<color=red>Failed to get template id {instanceId}</color>");
            
            return null;
        }

        public void CleanUp()
        {
            foreach (var view in spawnedViews)
            {
                gameObjectsPool.Release(view);
            }
            spawnedViews.Clear();
        }
    }
}