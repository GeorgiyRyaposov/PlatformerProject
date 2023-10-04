using UnityEngine;

namespace Game.Loaders
{
    [CreateAssetMenu(fileName = "TechnicalData", menuName = "ScriptableObjects/TechnicalData")]
    public class TechnicalData : ScriptableObject
    {
        [SerializeField] private string preloaderSceneName;
        public string PreloaderSceneName => preloaderSceneName;
        
        [SerializeField] private string objectsPoolSceneName = "ObjectsPool";
        public string ObjectsPoolSceneName => objectsPoolSceneName;
        
        [SerializeField] private string gameGuiSceneName = "GameGui";
        public string GameGuiSceneName => gameGuiSceneName;
    }
}