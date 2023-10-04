using UnityEngine;

namespace Common.GameObjects
{
    public class GameObjectTemplateHolder : MonoBehaviour
    {
        [SerializeField] private GameObjectTemplate template;
        public GameObjectTemplate Template => template;
    }
}