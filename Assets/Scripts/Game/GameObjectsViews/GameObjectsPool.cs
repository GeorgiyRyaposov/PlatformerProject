using System.Collections.Generic;
using Common.Data;
using Common.GameObjects;
using Common.ServiceLocator;
using Game.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.GameObjectsViews
{
    [CreateAssetMenu(fileName = "GameObjectsPool", menuName = "Services/GameObjectsPool")]
    public class GameObjectsPool : ScriptableObject, IInitializableService
    {
        [SerializeField] private GameObjectsTemplates templates;
        [SerializeField] private TechnicalData technicalData;

        private readonly Dictionary<Id, GameObjectTemplate> templatesMap = new();
        private readonly Dictionary<Id, Stack<GameObjectView>> pool = new();
        private readonly HashSet<GameObjectView> used = new();

        private Transform poolRoot;

        public void Initialize()
        {
            for (var i = 0; i < templates.Values.Count; i++)
            {
                var template = templates.Values[i];
                templatesMap[template.Id] = template;
            }
            
            CreatePoolRoot();
        }
        
        public void Dispose()
        {
            ClearAll();
        }
        
        public void OnLocationLoaded()
        {
            CreatePoolRoot();
        }

        private void CreatePoolRoot()
        {
            if (poolRoot != null)
            {
                return;
            }

            var scene = SceneManager.CreateScene(technicalData.ObjectsPoolSceneName);

            var gameObject = new GameObject("Pool");
            SceneManager.MoveGameObjectToScene(gameObject, scene);

            poolRoot = gameObject.transform;
        }

        public GameObjectView Get(Id templateId)
        {
            var gameObjectView = GetOrCreateGameObjectView(templateId);
            if (gameObjectView == null)
            {
                return null;
            }
            
            gameObjectView.TemplateId = templateId;
            used.Add(gameObjectView);
            return gameObjectView;
        }

        private GameObjectView GetOrCreateGameObjectView(Id templateId)
        {
            if (!pool.TryGetValue(templateId, out var stack))
            {
                stack = new Stack<GameObjectView>();
                pool[templateId] = stack;
            }

            if (stack.Count > 0)
            {
                var gameObjectView = stack.Pop();
                gameObjectView.gameObject.SetActive(true);
                return gameObjectView;
            }

            if (templatesMap.TryGetValue(templateId, out var template))
            {
                var random = Random.Range(0, template.ViewsTemplates.Count);
                var prefab = template.ViewsTemplates[random].Prefab;
                var gameObjectView = GameObject.Instantiate(prefab, poolRoot);
                return gameObjectView;
            }

            Debug.LogError($"<color=red>Failed to find view {templateId}</color>");
            return null;
        }

        public void Release(GameObjectView gameObjectView)
        {
            gameObjectView.gameObject.SetActive(false);
            
            var templateId = gameObjectView.TemplateId;
            
            if (!pool.TryGetValue(templateId, out var stack))
            {
                stack = new Stack<GameObjectView>();
                pool[templateId] = stack;
            }

            used.Remove(gameObjectView);
            stack.Push(gameObjectView);
        }

        private void ClearAll()
        {
            foreach (var gameObjectView in used)
            {
                if (gameObjectView)
                {
                    GameObject.Destroy(gameObjectView.gameObject);
                }
            }
            used.Clear();

            foreach (var stacks in pool.Values)
            {
                while (stacks.Count > 0)
                {
                    var gameObjectView = stacks.Pop();
                    if (gameObjectView)
                    {
                        GameObject.Destroy(gameObjectView.gameObject);
                    }
                }
            }
            pool.Clear();
        }
    }
}