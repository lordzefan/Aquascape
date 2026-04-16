using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class FileWatcher : MonoBehaviour
{
    private string folderPath;
    private HashSet<string> loadedFiles = new HashSet<string>();

    void Start()
    {
        folderPath = Application.dataPath + "/StreamingAssets/Input";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    void Update()
    {
        ScanFolder();
    }

    void ScanFolder()
    {
        var files = Directory.GetFiles(folderPath, "*.png");

        foreach (var file in files)
        {
            if (!loadedFiles.Contains(file))
            {
                loadedFiles.Add(file);
                SpawnManager.Instance.HandleNewFile(file);
            }
        }
    }
}