using System.Collections.Generic;
using UnityEngine;

namespace Common.GameObjects
{
    [CreateAssetMenu(fileName = "GameObject templates", menuName = "ScriptableObjects/GameObjectsTemplates")]
    public class GameObjectsTemplates : ScriptableObject
    {
        [SerializeField] private List<GameObjectTemplate> values;
        public List<GameObjectTemplate> Values => values;
    }
}