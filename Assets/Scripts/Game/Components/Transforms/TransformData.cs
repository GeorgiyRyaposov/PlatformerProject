using System;
using Common.Components;
using Common.Data;
using UnityEngine;

namespace Game.Components.Transforms
{
    [Serializable]
    public struct TransformData : IComponentData
    {
        public Id Id { get => id; set => id = value; }

        [SerializeField] private Id id;

        public TransformData(Id id, Transform transform)
        {
            this.id = id;
            
            Position = transform.position;
            Rotation = transform.rotation;
            Scale = transform.localScale;
        }
        
        public SerializableVector3 Position;
        public SerializableQuaternion Rotation;
        public SerializableVector3 Scale;
    }
}