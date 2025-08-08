using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_PATH = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SAVE_PATH, json);
        Debug.Log("Game saved to: " + SAVE_PATH);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(SAVE_PATH))
        {
            string json = File.ReadAllText(SAVE_PATH);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.Log("No save file found at: " + SAVE_PATH);
            return null;
        }
    }

    public static bool SaveExists()
    {
        return File.Exists(SAVE_PATH);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
            Debug.Log("Save file deleted");
        }
    }
}