using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public GameObject foodPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleLeftClick();
    }

    /// <summary>
    /// Handle left mouse click interactions.
    /// </summary>
    private void HandleLeftClick()
    {
        Vector2 worldPosition =
            Camera.main.ScreenToWorldPoint(
                Input.mousePosition
            );

        Collider2D hit =
            Physics2D.OverlapPoint(worldPosition);

        if (TryHandleFishClick(hit))
            return;

        if (TryHandleTrashClick(hit))
            return;

        SpawnFood(worldPosition);
    }

    /// <summary>
    /// Scare fish when clicked.
    /// </summary>
    private bool TryHandleFishClick(Collider2D hit)
    {
        if (hit == null)
            return false;

        Fish fish = hit.GetComponent<Fish>();

        if (fish == null)
            return false;

        fish.Scare();
        return true;
    }

    /// <summary>
    /// Destroy trash when clicked.
    /// </summary>
    private bool TryHandleTrashClick(Collider2D hit)
    {
        if (hit == null)
            return false;

        Trash trash = hit.GetComponent<Trash>();

        if (trash == null)
            return false;

        Destroy(trash.gameObject);
        return true;
    }

    /// <summary>
    /// Spawn food at clicked position.
    /// </summary>
    private void SpawnFood(Vector2 position)
    {
        GameObject food = Instantiate(
            foodPrefab,
            position,
            Quaternion.identity
        );

        food.transform.localScale = Vector3.zero;

        StartCoroutine(
            PopFood(food.transform)
        );
    }

    /// <summary>
    /// Play scale pop animation for spawned food.
    /// </summary>
    private IEnumerator PopFood(Transform target)
    {
        float duration = 0.15f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float scale = Mathf.Lerp(
                0f,
                1f,
                timer / duration
            );

            target.localScale =
                Vector3.one * scale;

            yield return null;
        }

        target.localScale = Vector3.one;
    }
}