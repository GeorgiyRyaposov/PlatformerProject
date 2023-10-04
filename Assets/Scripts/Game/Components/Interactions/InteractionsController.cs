using System.Collections.Generic;
using Common.Components;
using Common.Data;
using Common.GameObjects;
using Common.ServiceLocator;
using Game.Components.Interactions.Templates;
using Game.Data;
using Game.GameObjectsViews;
using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Components.Interactions
{
    [CreateAssetMenu(fileName = "InteractionsController", menuName = "Services/InteractionsController")]
    public class InteractionsController : ScriptableObject, IInitializableService, ITickable, IComponentsController
    {
        [SerializeField]
        private ScriptableId controllerId;
        public Id Id => controllerId.Value;
        
        [SerializeField] private GameObjectsTemplates templates;
        
        [SerializeField]
        private float interactionRadius;
        
        [SerializeField]
        private LayerMask interactionObjectsLayerMask;

        private IDataController dataController;

        private readonly Collider[] overlapResults = new Collider[1024];
        
        private readonly List<GameObjectData> interactionObjects = new();
        private GameObjectData closestInteractionObject;

        public void Initialize()
        {
            dataController = ServicesMediator.DataController;
            
            ServicesMediator.TickableService.Add(this);
        }
        public void Dispose()
        {
            ServicesMediator.TickableService.Remove(this);
        }

        public void Tick()
        {
            UpdateInteractionObjects();
            ListenInteractAction();
        }

        private void UpdateInteractionObjects()
        {
            interactionObjects.Clear();

            var closestDistance = float.MaxValue;
            var position = dataController.GetPlayerPosition();
            var hits = Physics.OverlapSphereNonAlloc(position, interactionRadius, overlapResults, interactionObjectsLayerMask);
            for (var i = 0; i < hits; i++)
            {
                var hit = overlapResults[i];
                if (!hit.gameObject.TryGetComponent<GameObjectView>(out var view))
                {
                    continue;
                }

                interactionObjects.Add(new GameObjectData(view));
                
                var distance = (position - hit.transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractionObject = interactionObjects[i];
                }
            }
        }

        private void ListenInteractAction()
        {
            if (!InputMediator.InputEventsHolder.Interact || interactionObjects.Count == 0)
            {
                return;
            }

            var template = GetTemplate(closestInteractionObject.TemplateId);
            if (template == null)
            {
                Debug.LogError($"<color=red>Failed to find template {closestInteractionObject.TemplateId}</color>");
                return;
            }
            
            if (!template.TryGetComponent<InteractionTemplate>(out var interactionTemplate))
            {
                Debug.LogError($"<color=red>Has no interaction template {closestInteractionObject.TemplateId}</color>");
                return;
            }

            if (!interactionTemplate.TryInteract(closestInteractionObject.InstanceId))
            {
                Debug.Log($"<color=yellow>Unable to interact</color>");
            }
        }

        private GameObjectTemplate GetTemplate(Id templateId)
        {
            return templates.Values.Find(x => x.Id == templateId);
        }
    }
}