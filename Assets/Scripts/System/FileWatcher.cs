using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class FileWatcher : MonoBehaviour
{
    private string folderPath;

    private readonly HashSet<string> loadedFiles =
        new HashSet<string>();

    private void Start()
    {
        InitializeFolder();
    }

    private void Update()
    {
        ScanFolder();
    }

    /// <summary>
    /// Create input folder if it does not exist.
    /// </summary>
    private void InitializeFolder()
    {
        folderPath = Path.Combine(
            Application.dataPath,
            "StreamingAssets/Input"
        );

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    /// <summary>
    /// Scan folder for new PNG files.
    /// </summary>
    private void ScanFolder()
    {
        string[] files =
            Directory.GetFiles(folderPath, "*.png");

        foreach (string file in files)
        {
            if (loadedFiles.Contains(file))
                continue;

            loadedFiles.Add(file);

            SpawnManager.Instance.HandleNewFile(file);
        }
    }
}