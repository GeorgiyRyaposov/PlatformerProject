using System;
using Common.Views;

namespace Common.Data
{
    [Serializable]
    public struct GameObjectData
    {
        public Id InstanceId;
        public Id TemplateId;

        public GameObjectData(IGameObjectView view)
        {
            InstanceId = view.InstanceId;
            TemplateId = view.TemplateId;
        }
        
        public GameObjectData(Id instanceId, Id templateId)
        {
            InstanceId = instanceId;
            TemplateId = templateId;
        }
    }
}