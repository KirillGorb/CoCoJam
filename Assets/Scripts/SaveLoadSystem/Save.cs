using System.IO;
using CodeScripts.Abstraction;
using UnityEngine;

namespace CodeScripts.SaveLoadSystem
{
    public class Save<T> where T : IData
    {
        private readonly string _filePath;

        private Save()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        }

        public void SaveData(T data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_filePath, json);
            Debug.Log("Data saved to " + _filePath);
        }

        public T LoadData()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log("Data loaded from " + _filePath);
                return data;
            }

            Debug.LogError("Save file not found in " + _filePath);
            return default;
        }
    }
}