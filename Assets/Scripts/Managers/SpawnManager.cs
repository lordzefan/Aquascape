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

    void Awake()
    {
        Instance = this;
    }

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

        if (category == "FISH")
        {
            SpawnFish(texture, type);
        }
        else if (category == "TRASH")
        {
            SpawnTrash(texture, type);
        }
        else
        {
            Debug.LogWarning("Unknown category: " + category);
        }
    }

    Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path)) return null;

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);

        tex.LoadImage(bytes);
        return tex;
    }

    void SpawnFish(Texture2D tex, string type)
    {
        GameObject obj = Instantiate(fishPrefab, GetRandomPosition(), Quaternion.identity, entityParent);

        ApplyTexture(obj, tex);
    }

    void SpawnTrash(Texture2D tex, string type)
    {
        GameObject obj = Instantiate(trashPrefab, GetRandomPosition(), Quaternion.identity, entityParent);

        ApplyTexture(obj, tex);
    }

    void ApplyTexture(GameObject obj, Texture2D tex)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        sr.sprite = sprite;
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
    }
}