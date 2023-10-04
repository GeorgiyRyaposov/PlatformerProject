using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Loaders
{
    [CreateAssetMenu(fileName = "SceneUnloader", menuName = "Services/SceneUnloader")]
    public class SceneUnloader : ScriptableObject, IService
    {
        [SerializeField] private TechnicalData technicalData;
        private readonly List<ICleanUpOnLocationUnload> onUnloadCleaners = new();
        
        public async Task Unload()
        {
            onUnloadCleaners.Clear();
            Global.ServiceLocator.CollectAll(onUnloadCleaners);
            foreach (var cleanUpOnLocationUnload in onUnloadCleaners)
            {
                cleanUpOnLocationUnload.CleanUp();
            }
            
            var count = SceneManager.sceneCount;
            var operations = new List<AsyncOperation>(count);
            
            for (var i = 0; i < count; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.IsValid())
                {
                    continue;
                }

                if (string.Equals(scene.name, technicalData.PreloaderSceneName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(scene.name, technicalData.ObjectsPoolSceneName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                
                var op = SceneManager.UnloadSceneAsync(scene);
                operations.Add(op);
            }

            while (!IsAllOperationsDone(operations))
            {
                await Task.Yield();
            }
        }

        private bool IsAllOperationsDone(List<AsyncOperation> operations)
        {
            for (var i = 0; i < operations.Count; i++)
            {
                if (!operations[i].isDone)
                {
                    return false;
                }
            }

            return true;
        }
    }
}