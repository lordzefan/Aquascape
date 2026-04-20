using UnityEngine;
using System.IO;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Prefabs")]
    public GameObject fishPrefab;
    public GameObject trashPrefab;

    [Header("Parent")]
    public Transform entityParent;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Process a newly detected file and spawn the correct entity.
    /// </summary>
    public void HandleNewFile(string path)
    {
        string fileName = Path.GetFileNameWithoutExtension(path);
        string[] parts = fileName.Split('_');

        if (parts.Length < 3)
        {
            Debug.LogWarning("Invalid file name: " + fileName);
            return;
        }

        string category = parts[0];
        string type = parts[1];

        Texture2D texture = LoadTexture(path);

        if (texture == null)
        {
            Debug.LogError("Failed to load texture: " + path);
            return;
        }

        SpawnByCategory(category, texture, type);
    }

    /// <summary>
    /// Load image file as Texture2D.
    /// </summary>
    private Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path))
            return null;

        byte[] bytes = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        return texture;
    }

    /// <summary>
    /// Spawn entity based on file category.
    /// </summary>
    private void SpawnByCategory(
        string category,
        Texture2D texture,
        string type
    )
    {
        switch (category)
        {
            case "FISH":
                SpawnFish(texture, type);
                break;

            case "TRASH":
                SpawnTrash(texture, type);
                break;

            default:
                Debug.LogWarning("Unknown category: " + category);
                break;
        }
    }

    /// <summary>
    /// Spawn fish entity.
    /// </summary>
    private void SpawnFish(Texture2D texture, string type)
    {
        GameObject obj = Instantiate(
            fishPrefab,
            GetRandomPosition(),
            Quaternion.identity,
            entityParent
        );

        ApplyTexture(obj, texture);
    }

    /// <summary>
    /// Spawn trash entity.
    /// </summary>
    private void SpawnTrash(Texture2D texture, string type)
    {
        GameObject obj = Instantiate(
            trashPrefab,
            GetRandomPosition(),
            Quaternion.identity,
            entityParent
        );

        ApplyTexture(obj, texture);
    }

    /// <summary>
    /// Apply texture as sprite to spawned object.
    /// </summary>
    private void ApplyTexture(GameObject obj, Texture2D texture)
    {
        SpriteRenderer spriteRenderer =
            obj.GetComponent<SpriteRenderer>();

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        spriteRenderer.sprite = sprite;
    }

    /// <summary>
    /// Generate random spawn position.
    /// </summary>
    private Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(-5f, 5f),
            Random.Range(-3f, 3f)
        );
    }
}