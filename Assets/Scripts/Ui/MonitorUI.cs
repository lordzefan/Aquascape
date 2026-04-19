using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonitorUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monitorText;

    private float deltaTime = 0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        RefreshUI();
    }

    void RefreshUI()
    {
        int fishCount = FindObjectsOfType<Fish>().Length;
        int trashCount = FindObjectsOfType<Trash>().Length;
        int foodCount = FindObjectsOfType<Food>().Length;

        float fps = 1f / deltaTime;

        string configStatus =
            ConfigManager.Data != null ? "YES" : "NO";

        monitorText.text =
            "=== AQUARIUM MONITOR ===\n\n" +
            "Fish Count : " + fishCount + "\n" +
            "Trash Count: " + trashCount + "\n" +
            "Food Count : " + foodCount + "\n\n" +
            "FPS        : " + Mathf.Ceil(fps) + "\n" +
            "Config OK  : " + configStatus;
    }
}