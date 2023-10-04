using Game.GameObjectsViews;
using UnityEngine;

namespace Common.Views
{
    [CreateAssetMenu(fileName = "View template", menuName = "ScriptableObjects/View")]
    public class ViewTemplate : ScriptableObject
    {
        [SerializeField] private GameObjectView prefab;

        public GameObjectView Prefab => prefab;
    }
}