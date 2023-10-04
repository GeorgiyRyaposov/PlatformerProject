using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    [CreateAssetMenu(fileName = "ScriptablesHolder", menuName = "ScriptableObjects/ScriptablesHolder")]
    public class ScriptablesHolder : ScriptableObject
    {
        [SerializeField]
        private List<ScriptableObject> values;
        public List<ScriptableObject> Values => values;
    }
}