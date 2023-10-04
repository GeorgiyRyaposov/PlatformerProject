using System.Collections.Generic;
using Common.Data;
using Common.GameObjects;
using Game.Components.Transforms;
using UnityEngine;

namespace Game.Data.Scenes
{
    [CreateAssetMenu(fileName = "LocationScenes", menuName = "ScriptableObjects/LocationScenes")]
    public class LocationScenes : ScriptableObject
    {
        [SerializeField, HideInInspector] private Id id;
        public Id Id => id;
        
        [SerializeField] private List<string> scenes;
        public List<string> Scenes => scenes;
        
        [SerializeField] 
        private SerializableVector3 playerSpawnPosition;
        public SerializableVector3 PlayerSpawnPosition => playerSpawnPosition;
        
        [SerializeField] 
        private SerializableVector3 scenesCenter;
        [SerializeField] 
        private SerializableVector3 scenesSize;
        
        public Bounds SceneBounds => new Bounds(scenesCenter, scenesSize);
        
        [SerializeField] private LocationGameObjects locationGameObjects;
        public LocationGameObjects LocationGameObjects => locationGameObjects;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (id.IsZero)
            {
                id = Id.Create();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        
        private void SetLocationGameObjects(LocationGameObjects value)
        {
            locationGameObjects = value;
        }

        private void SetScenesBounds(Bounds bounds)
        {
            scenesCenter = bounds.center;
            scenesSize = bounds.size;
        }
        
        [UnityEditor.CustomEditor(typeof(LocationScenes))]
        public class LocationScenesEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                if (GUILayout.Button("Update game objects"))
                {
                    var data = (LocationScenes)target;

                    CollectGameObjects(data);
                    UpdateSceneBounds(data);
                    GetSpawnPoint(data);
                    
                    UnityEditor.EditorUtility.SetDirty(target);
                }

                DrawDefaultInspector();
            }

            private void GetSpawnPoint(LocationScenes data)
            {
                var respawn = GameObject.FindWithTag("Respawn");
                if (respawn != null)
                {
                    data.playerSpawnPosition = respawn.transform.position;
                }
            }

            private void UpdateSceneBounds(LocationScenes data)
            {
                var renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
                if (renderers.Length == 0)
                {
                    return;
                }
                
                var bounds = renderers[0].bounds;
                for (int i = 1; i < renderers.Length; i++)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
                
                data.SetScenesBounds(bounds);
            }

            private static void CollectGameObjects(LocationScenes data)
            {
                var templateHolders = FindObjectsByType<GameObjectTemplateHolder>(FindObjectsSortMode.None);
                var result = new LocationGameObjects();
                foreach (var templateHolder in templateHolders)
                {
                    var objectId = Id.Create();

                    var transform = templateHolder.transform;
                    result.TransformData.Add(new TransformData()
                    {
                        Id = objectId,
                        Position = transform.position,
                        Rotation = transform.rotation,
                        Scale = transform.localScale,
                    });

                    result.GameObjectInstances.Add(new GameObjectInstance()
                    {
                        InstanceId = objectId,
                        TemplateId = templateHolder.Template.Id,
                    });
                }

                data.SetLocationGameObjects(result);
            }
        }
        
        
#endif
    }
}