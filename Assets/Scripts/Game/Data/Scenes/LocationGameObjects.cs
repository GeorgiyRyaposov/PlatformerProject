using System.Collections.Generic;
using Common.Data;
using Game.Components.Transforms;

namespace Game.Data.Scenes
{
    [System.Serializable]
    public class LocationGameObjects
    {
        public List<TransformData> TransformData = new();
        public List<GameObjectInstance> GameObjectInstances = new();
    }

    [System.Serializable]
    public struct GameObjectInstance
    {
        public Id InstanceId;
        public Id TemplateId;
    }
}