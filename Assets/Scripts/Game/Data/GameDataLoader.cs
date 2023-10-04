using System.IO;
using Common.ServiceLocator;
using UnityEngine;

namespace Game.Data
{
    public class GameDataLoader : IService
    {
        private string SaveFileName = "Save.json";
        
        public void SaveData()
        {
            var gameModel = ServicesMediator.GameModelHolder.GetSerializableGameModel();
            
            var path = GetSavePath();
            var saveData = new SaveData
            {
                GameModelWrapper = gameModel
            };
            
            var json = JsonUtility.ToJson(saveData);
            
            File.WriteAllText(path, json);
            Debug.Log($"<color=green>Game saved {path}</color>");
        }

        public SaveData LoadData()
        {
            var path = GetSavePath();
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        
        public bool HasSavedGame()
        {
            return File.Exists(GetSavePath());
        }
        
        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }
    }
}