using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject foodPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
    }

    void HandleLeftClick()
    {
        Vector2 worldPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit =
            Physics2D.OverlapPoint(worldPos);

        // ==========================
        // CLICK FISH
        // ==========================
        if (hit != null)
        {
            Fish fish = hit.GetComponent<Fish>();

            if (fish != null)
            {
                fish.Scare();
                return;
            }

            // ==========================
            // CLICK TRASH
            // ==========================
            Trash trash = hit.GetComponent<Trash>();

            if (trash != null)
            {
                Destroy(trash.gameObject);
                return;
            }
        }

        // ==========================
        // CLICK EMPTY SPACE
        // ==========================
        Instantiate(foodPrefab, worldPos, Quaternion.identity);
    }
}