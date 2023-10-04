using System;
using System.Collections.Generic;
using System.Linq;
using Common.Components;
using Common.Data;
using Game.Components.Transforms;

namespace Game.Data
{
    [Serializable]
    public class SaveData
    {
        public SerializableGameModel GameModelWrapper;
    }
    
    [Serializable]
    public class SerializableGameModel
    {
        public SerializableGameModel() { }

        public SerializableGameModel(GameModel gameModel)
        {
            CurrentLocation = gameModel.CurrentLocation;
            
            Components = gameModel.Components.Select(x => new ComponentsServiceData(x.Key, x.Value)).ToList();
            Transforms = gameModel.Transforms.Values.ToList();
            GameObjectsInstancesToTemplates = gameModel.GameObjectsInstancesToTemplatesMap
                .Select(x => new GameObjectData(x.Key, x.Value)).ToList();
            
            PlayerPosition = gameModel.PlayerPosition;
        }

        public GameModel ToGameModel()
        {
            var gameModel = new GameModel()
            {
                CurrentLocation = CurrentLocation,

                Components = Components.ToDictionary(k => k.ServiceId, v => v.Data),
                Transforms = Transforms.ToDictionary(k => k.Id, v => v),
                GameObjectsInstancesToTemplatesMap = GameObjectsInstancesToTemplates
                    .ToDictionary(k => k.InstanceId, v => v.TemplateId),
                PlayerPosition = PlayerPosition,
            };
                
            return gameModel;
        }
        
        public Id CurrentLocation;
        
        public List<ComponentsServiceData> Components = new ();
        public List<TransformData> Transforms = new ();
        
        public List<GameObjectData> GameObjectsInstancesToTemplates = new ();
        public SerializableVector3 PlayerPosition;
    }

    [Serializable]
    public struct ComponentsServiceData
    {
        public Id ServiceId;
        public IComponentDataDictionary Data;

        public ComponentsServiceData(Id serviceId, IComponentDataDictionary data)
        {
            ServiceId = serviceId;
            Data = data;
        }
    }
}