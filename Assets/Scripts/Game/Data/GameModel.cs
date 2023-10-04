using System;
using System.Collections.Generic;
using Common.Components;
using Common.Data;
using Game.Components.Transforms;

namespace Game.Data
{
    public class GameModel
    {
        public Id CurrentLocation;
        
        public Dictionary<Id, IComponentDataDictionary> Components = new ();
        public Dictionary<Id, TransformData> Transforms = new ();
        
        public Dictionary<Id, Id> GameObjectsInstancesToTemplatesMap = new ();
        public SerializableVector3 PlayerPosition;
    }
}