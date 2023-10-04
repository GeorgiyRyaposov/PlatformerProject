using Common.Data;
using DG.Tweening;
using UnityEngine;

namespace Game.Components.Interactions.Templates
{
    [CreateAssetMenu(fileName = "Test Interaction", menuName = "ScriptableObjects/TestInteraction")]
    public class TestInteraction : InteractionTemplate
    {
        [SerializeField] private float punchPower;
        [SerializeField] private float punchDuration;
        
        public override bool CanInteract(Id instanceId)
        {
            return true;
        }

        public override bool TryInteract(Id instanceId)
        {
            if (!CanInteract(instanceId))
            {
                return false;
            }

            var view = ServicesMediator.GameObjectsController.GetGameObjectView(instanceId);
            if (view == null)
            {
                Debug.LogError($"<color=red>Failed to find view for {instanceId}</color>");
                return false;
            }

            var colliders = view.GetComponents<Collider>();
            EnableColliders(colliders, false);
            
            view.transform.DOPunchScale(Vector3.up * punchPower, punchDuration, 1)
                .OnComplete(() =>
                {
                    view.transform.localScale = Vector3.one;
                    EnableColliders(colliders, true);
                });
            
            return true;
        }

        private static void EnableColliders(Collider[] colliders, bool enable)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = enable;
            }
        }
    }
}