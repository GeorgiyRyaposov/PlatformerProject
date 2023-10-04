using System.Collections.Generic;
using Common.Components;
using Common.Data;
using Common.ServiceLocator;
using Game.Components.Transforms;
using Game.Signals;
using UnityEngine;

namespace Game.Data
{
    public class DataController : IDataController, IGameModelHolder, IService
    {
        private GameModel model;
        
        public void SetModel(GameModel model)
        {
            this.model = model;
        }

        public SerializableGameModel GetSerializableGameModel()
        {
            return new SerializableGameModel(model);
        }
        public bool HasModel() => model != null;

        public IComponentDataDictionary GetComponentsData(Id controllerId)
        {
            if (model.Components.TryGetValue(controllerId, out var data))
            {
                return data;
            }

            Debug.LogError($"<color=red>Failed to find data for {controllerId}</color>");
            return null;
        }

        public void SetComponentsData(Id controllerId, IComponentDataDictionary data)
        {
            model.Components[controllerId] = data;
        }

        public void SetCurrentLocation(Id locationId)
        {
            model.CurrentLocation = locationId;
            ServicesMediator.Signals.Fire<LocationChanged>();
        }

        public Id GetCurrentLocation()
        {
            return model.CurrentLocation;
        }

        public Dictionary<Id, Id> GetGameObjectsToTemplatesMap()
        {
            return model.GameObjectsInstancesToTemplatesMap;
        }

        public TransformData GetTransform(Id objectId)
        {
            return model.Transforms[objectId];
        }
        public void SetTransform(Id objectId, TransformData data)
        {
            model.Transforms[objectId] = data;
        }

        public void SetPlayerPosition(Vector3 position)
        {
            model.PlayerPosition = position;
        }

        public Vector3 GetPlayerPosition()
        {
            return model.PlayerPosition;
        }
    }
}