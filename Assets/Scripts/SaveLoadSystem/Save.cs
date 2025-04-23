using System.IO;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;

namespace CodeScripts.SaveLoadSystem
{
    public class Save<T> where T : class
    {
        private string _filePath;

        [MenuItem("Tools/Delete")]
        public void Delete() => File.Delete(Application.persistentDataPath);

        public void SetSave(string nameFile)
        {
            _filePath = Path.Combine(Application.persistentDataPath, $"{nameFile}.json");
        }

        public void SaveData(T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_filePath, json);
            Debug.Log("Data saved to " + _filePath);
        }

        public T LoadData()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                T data = JsonConvert.DeserializeObject<T>(json);
                Debug.Log("Data loaded from " + _filePath);
                return data;
            }

            Debug.LogError("Save file not found in " + _filePath);
            return null;
        }
    }
}