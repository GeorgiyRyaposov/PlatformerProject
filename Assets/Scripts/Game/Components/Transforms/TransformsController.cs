using System.Collections.Generic;
using Common.Components;
using Common.Data;
using Common.ServiceLocator;
using Game.Data;
using Game.GameObjectsViews;
using UnityEngine;
using Zenject;

namespace Game.Components.Transforms
{
    [CreateAssetMenu(fileName = "TransformsController", menuName = "Services/TransformsController")]
    public class TransformsController : ScriptableObject, IComponentsController, ITickable, IInitializableService
    {
        [SerializeField]
        private ScriptableId controllerId;
        
        public Id Id => controllerId.Value;

        private IDataController dataController;
        private GameObjectsController gameObjectsController;

        private readonly List<GameObjectView> spawnedViews = new();

        public void Initialize()
        {
            dataController = ServicesMediator.DataController;
            gameObjectsController = ServicesMediator.GameObjectsController;
            
            ServicesMediator.TickableService.Add(this);
        }

        public void Dispose()
        {
            ServicesMediator.TickableService?.Remove(this);
        }

        public TransformData GetTransformData(Id instanceId)
        {
            return dataController.GetTransform(instanceId);
        }

        public void Tick()
        {
            SyncTransformData();
        }

        private void SyncTransformData()
        {
            UpdateViewsList();

            for (var i = 0; i < spawnedViews.Count; i++)
            {
                var view = spawnedViews[i];
                if (view == null)
                {
                    continue;
                }

                var viewTransform = view.transform;
                if (!viewTransform.hasChanged)
                {
                    continue;
                }

                view.transform.hasChanged = false;

                var data = new TransformData(view.InstanceId, viewTransform);
                dataController.SetTransform(data.Id, data);
            }
        }

        private void UpdateViewsList()
        {
            if (spawnedViews.Count == gameObjectsController.SpawnedViewsCount)
            {
                return;
            }
            spawnedViews.Clear();
            gameObjectsController.CollectSpawnedViews(spawnedViews);
        }
    }
}