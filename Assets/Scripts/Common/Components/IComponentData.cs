using System;
using System.Collections.Generic;
using Common.Data;

namespace Common.Components
{
    public interface IComponentData
    {
        Id Id { get; set; }
    }

    public interface IComponentDataDictionary
    {
        public Dictionary<Id, IComponentData> Data { get; }
    }
}