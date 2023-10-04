using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Data;
using Common.ServiceLocator;
using Game.Data.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Loaders
{
    [CreateAssetMenu(fileName = "SceneLoader", menuName = "Services/SceneLoader")]
    public class SceneLoader : ScriptableObject, IService
    {
        [SerializeField] private LocationsHolder locationsHolder;

        public async Task Load(Id locationId)
        {
            var scenesData = locationsHolder.Values.Find(x => x.Id == locationId);
            if (scenesData == null)
            {
                Debug.LogError($"<color=red>Failed to find scene {locationId}</color>");
                return;
            }

            var asyncOperations = new List<AsyncOperation>(scenesData.Scenes.Count);
            for (var i = 0; i < scenesData.Scenes.Count; i++)
            {
                var sceneData = scenesData.Scenes[i];
                
                var op = SceneManager.LoadSceneAsync(sceneData, LoadSceneMode.Additive);
                op.allowSceneActivation = true;
                asyncOperations.Add(op);
            }
            
            while (!IsAllOperationsDone(asyncOperations))
            {
                await Task.Yield();
            }
        }

        private bool IsAllOperationsDone(List<AsyncOperation> asyncOperations)
        {
            for (var i = 0; i < asyncOperations.Count; i++)
            {
                if (!asyncOperations[i].isDone)
                {
                    return false;
                }
            }

            return true;
        }
    }
}