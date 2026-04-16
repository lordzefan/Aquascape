using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject foodPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Instantiate(foodPrefab, worldPos, Quaternion.identity);
    }
}