using Common.Data;
using UnityEngine;

namespace Common.Components
{
    public class ComponentTemplate : ScriptableObject
    {
        [SerializeField] private ScriptableId componentsControllerId;
        public ScriptableId ComponentsControllerId => componentsControllerId;
    }
}