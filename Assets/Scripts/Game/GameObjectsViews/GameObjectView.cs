using Common.Data;
using Common.Views;
using UnityEngine;

namespace Game.GameObjectsViews
{
    public class GameObjectView : MonoBehaviour, IGameObjectView
    {
        public Id TemplateId { get; set; }
        public Id InstanceId { get; set; }
    }
}