using System.Threading.Tasks;
using Common.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Loaders
{
    [CreateAssetMenu(fileName = "PreloaderController", menuName = "Services/PreloaderController")]
    public class PreloaderController : ScriptableObject, IService
    {
        [SerializeField] private TechnicalData technicalData;
        
        public PreloaderController(TechnicalData technicalData)
        {
            this.technicalData = technicalData;
        }
        
        public async Task Show()
        {
            var op = SceneManager.LoadSceneAsync(technicalData.PreloaderSceneName, LoadSceneMode.Additive);
            op.allowSceneActivation = true;
            while (!op.isDone)
            {
                await Task.Yield();
            }
        }
        
        public async Task Hide()
        {
            var op = SceneManager.UnloadSceneAsync(technicalData.PreloaderSceneName);
            while (!op.isDone)
            {
                await Task.Yield();
            }
        }
    }
}