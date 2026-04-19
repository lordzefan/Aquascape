using UnityEngine;
using System.IO;

public class ConfigManager : MonoBehaviour
{
    public static ConfigData Data;

    void Awake()
    {
        LoadConfig();
    }

    void LoadConfig()
    {
        string path =
            Application.streamingAssetsPath + "/config.json";

        if (!File.Exists(path))
        {
            Debug.LogError("Config file not found");
            return;
        }

        string json = File.ReadAllText(path);

        Data = JsonUtility.FromJson<ConfigData>(json);

        Debug.Log("Config Loaded");
    }
}

[System.Serializable]
public class ConfigData
{
    public float fishMinSpeed;
    public float fishMaxSpeed;
    public float fishDetectionRadius;
    public float fishHungerDecreaseRate;

    public float trashMinSpeed;
    public float trashMaxSpeed;
}