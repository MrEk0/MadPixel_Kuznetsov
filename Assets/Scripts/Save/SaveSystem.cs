using System;
using System.IO;
using UnityEngine;

namespace Save
{
    public class SaveSystem
    {
        private static readonly string SAVE_PATH = Path.Combine(Application.persistentDataPath, "save.json");

        private SaveData _saveData;

        public SaveData SaveData => _saveData;

        public SaveSystem()
        {
            _saveData = ReadFromFile();
        }

        public void Save()
        {
            if (TrySerializeProfileData(_saveData, out var profileDataJson))
            {
                WriteToFile(profileDataJson);
            }
        }

        private SaveData ReadFromFile()
        {
            if (!File.Exists(SAVE_PATH))
                return new SaveData();

            return TryReadFile(SAVE_PATH, out var saveData) ? saveData : new SaveData();
        }

        private bool TryReadFile(string profileFilePath, out SaveData saveData)
        {
            var profileDataJson = File.ReadAllText(profileFilePath);

            if (!string.IsNullOrEmpty(profileDataJson))
                return TryDeserializeProfileData(profileDataJson, out saveData);

            saveData = null;
            return false;
        }

        private void WriteToFile(string profileDataJson)
        {
            if (!Directory.Exists(Application.persistentDataPath))
                Directory.CreateDirectory(Application.persistentDataPath);

            try
            {
                File.WriteAllText(SAVE_PATH, profileDataJson);
            }
            catch (Exception e)
            {
                Debug.LogError($"Main profile write exception: {e}");
            }
        }

        private bool TryDeserializeProfileData(string json, out SaveData saveData)
        {
            try
            {
                saveData = JsonUtility.FromJson<SaveData>(json);
                return saveData != null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Deserializing profile error: {ex}");

                saveData = new SaveData();
                return false;
            }
        }

        private bool TrySerializeProfileData(SaveData saveData, out string json)
        {
            json = string.Empty;

            try
            {
                json = JsonUtility.ToJson(saveData, false);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return false;
            }
        }
    }
}
