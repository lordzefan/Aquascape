using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonitorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monitorText;
    [SerializeField] private GameObject panelRoot;

    private float deltaTime = 0f;
    private bool isVisible = true;

    private void Update()
    {
        HandleToggle();

        if (!isVisible)
            return;

        UpdateDeltaTime();
        RefreshUI();
    }

    /// <summary>
    /// Toggle monitor panel visibility.
    /// </summary>
    private void HandleToggle()
    {
        if (!Input.GetKeyDown(KeyCode.F1))
            return;

        isVisible = !isVisible;

        panelRoot.SetActive(isVisible);
    }

    /// <summary>
    /// Smooth FPS delta time calculation.
    /// </summary>
    private void UpdateDeltaTime()
    {
        deltaTime +=
            (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    /// <summary>
    /// Refresh monitor information text.
    /// </summary>
    private void RefreshUI()
    {
        int fishCount = FindObjectsOfType<Fish>().Length;
        int trashCount = FindObjectsOfType<Trash>().Length;
        int foodCount = FindObjectsOfType<Food>().Length;

        float fps = 1f / deltaTime;

        string configStatus =
            ConfigManager.Data != null
                ? "YES"
                : "NO";

        monitorText.text =
            "=== AQUARIUM MONITOR ===\n\n" +
            "Fish Count : " + fishCount + "\n" +
            "Trash Count: " + trashCount + "\n" +
            "Food Count : " + foodCount + "\n\n" +
            "FPS        : " + Mathf.Ceil(fps) + "\n" +
            "Config OK  : " + configStatus + "\n\n" +
            "[F1] Toggle Panel";
    }
}