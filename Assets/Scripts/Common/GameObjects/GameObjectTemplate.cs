using System.Collections.Generic;
using Common.Components;
using Common.Data;
using Common.Views;
using UnityEngine;

namespace Common.GameObjects
{
    [CreateAssetMenu(fileName = "GameObject template", menuName = "ScriptableObjects/GameObject")]
    public class GameObjectTemplate : ScriptableObject
    {
        [SerializeField] private Id id;
        public Id Id => id;
        
        [SerializeField] private List<ViewTemplate> viewsTemplates;
        public List<ViewTemplate> ViewsTemplates => viewsTemplates;
        
        [SerializeField] private List<ComponentTemplate> componentsTemplates;
        public List<ComponentTemplate> ComponentsTemplates => componentsTemplates;

        public bool TryGetComponent<T>(out T component) where T : ComponentTemplate
        {
            for (var i = 0; i < componentsTemplates.Count; i++)
            {
                if (componentsTemplates[i] is T result)
                {
                    component = result;
                    return true;
                }
            }

            component = default;
            return false;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (id.IsZero)
            {
                id = Id.Create();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}