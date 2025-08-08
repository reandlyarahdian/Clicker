using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_PATH = Application.persistentDataPath + "/save.json";
    private const string SAVE_KEY = "CookieClickerSaveData";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

#if UNITY_WEBGL && !UNITY_EDITOR
        // For WebGL builds
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
#else
        // for other platform
        File.WriteAllText(SAVE_PATH, json);
        Debug.Log("Game saved to: " + SAVE_PATH);
#endif
    }

    public static SaveData LoadGame()
    {
       string json;
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        // For WebGL builds
        json = PlayerPrefs.GetString(SAVE_KEY, string.Empty);
        #else
        // For other platforms
        if (File.Exists(SAVE_PATH))
        {
            json = File.ReadAllText(SAVE_PATH);
        }
        else
        {
            json = string.Empty;
        }
        #endif

        if (!string.IsNullOrEmpty(json))
        {
            return JsonUtility.FromJson<SaveData>(json);
        }
        
        Debug.Log("No save data found");
        return null;
    }

    public static bool SaveExists()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // For WebGL builds
        return PlayerPrefs.HasKey(SAVE_KEY);
#else
        // for other platform
        return File.Exists(SAVE_PATH);
#endif
    }

    public static void DeleteSave()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // For WebGL builds
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
#else
        // for other platform
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
            Debug.Log("Save file deleted");
        }
#endif
    }
}