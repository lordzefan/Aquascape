using UnityEngine;
using System.IO;

public class ConfigManager : MonoBehaviour
{
    public static ConfigData Data;

    private string rootPath;
    private string streamingPath;

    private void Awake()
    {
        InitializePaths();
        EnsureConfigExists();
        LoadConfig();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ReloadConfig();
    }

    /// <summary>
    /// Initialize configuration file paths.
    /// </summary>
    private void InitializePaths()
    {
        rootPath = Path.Combine(
            Application.dataPath,
            "../config.json"
        );

        streamingPath = Path.Combine(
            Application.streamingAssetsPath,
            "config.json"
        );
    }

    /// <summary>
    /// Copy default config file if root config does not exist.
    /// </summary>
    private void EnsureConfigExists()
    {
        try
        {
            if (File.Exists(rootPath))
                return;

            if (File.Exists(streamingPath))
            {
                File.Copy(streamingPath, rootPath);
                Debug.Log(
                    "Config copied to root: " + rootPath
                );
            }
            else
            {
                Debug.LogError(
                    "Default config not found in StreamingAssets."
                );
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogError(
                "Error copying config: " +
                exception.Message
            );
        }
    }

    /// <summary>
    /// Load configuration data from file.
    /// </summary>
    private void LoadConfig()
    {
        try
        {
            if (!File.Exists(rootPath))
            {
                Debug.LogError(
                    "Config file not found at root."
                );
                return;
            }

            string json = File.ReadAllText(rootPath);

            Data = JsonUtility.FromJson<ConfigData>(json);

            Debug.Log(
                "Config loaded from: " + rootPath
            );
        }
        catch (System.Exception exception)
        {
            Debug.LogError(
                "Error loading config: " +
                exception.Message
            );
        }
    }

    /// <summary>
    /// Reload configuration while the game is running.
    /// </summary>
    public void ReloadConfig()
    {
        LoadConfig();
        Debug.Log("Config reloaded.");
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