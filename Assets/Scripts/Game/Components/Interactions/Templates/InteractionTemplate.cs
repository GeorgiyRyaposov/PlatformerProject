using Common.Components;
using Common.Data;
using UnityEngine;

namespace Game.Components.Interactions.Templates
{
    [CreateAssetMenu(fileName = "Interaction template", menuName = "ScriptableObjects/Interaction")]
    public class InteractionTemplate : ComponentTemplate
    {
        [SerializeField] private string nameKey;
        public string NameKey => nameKey;
        
        [SerializeField] private string descriptionKey;
        public string DescriptionKey => descriptionKey;

        public virtual bool TryInteract(Id instanceId)
        {
            return false;
        }

        public virtual bool CanInteract(Id instanceId)
        {
            return false;
        }
    }
}