using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PrefabData
{
    public GameObject prefab;
    [Range(0f, 0.1f)]
    public float density;
    [Range(0f, 5f)]
    public float minDistance;
}

public class PrefabGenerator : MonoBehaviour
{
    public PrefabData[] prefabData;
    public float renderDistance = 20f;
    public int gridSize = 128; // Size of the grid and the overall map

    private Dictionary<Vector2Int, GameObject> spawnedPrefabs;
    private Transform playerTransform;

    private void Awake()
    {
        spawnedPrefabs = new Dictionary<Vector2Int, GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        GenerateInitialGrid();
    }

    private void Update()
    {
        RenderPrefabsInRange();
    }

    private void GenerateInitialGrid()
    {
        Vector2Int playerTile = GetTilePosition(playerTransform.position);
        int halfGridSize = gridSize / 2;

        int startX = playerTile.x - halfGridSize;
        int startY = playerTile.y - halfGridSize;
        int endX = playerTile.x + halfGridSize;
        int endY = playerTile.y + halfGridSize;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);
                GameObject selectedPrefab = GetRandomPrefab();

                if (selectedPrefab != null && ShouldSpawnPrefab(selectedPrefab, tilePos))
                {
                    GameObject prefabInstance = Instantiate(selectedPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                    prefabInstance.SetActive(false);
                    spawnedPrefabs.Add(tilePos, prefabInstance);
                }
            }
        }
    }

    private void RenderPrefabsInRange()
    {
        Vector2Int playerTile = GetTilePosition(playerTransform.position);

        int minX = playerTile.x - Mathf.CeilToInt(renderDistance);
        int maxX = playerTile.x + Mathf.CeilToInt(renderDistance);
        int minY = playerTile.y - Mathf.CeilToInt(renderDistance);
        int maxY = playerTile.y + Mathf.CeilToInt(renderDistance);

        int startX = Mathf.Max(minX, -gridSize / 2);
        int startY = Mathf.Max(minY, -gridSize / 2);
        int endX = Mathf.Min(maxX, gridSize / 2);
        int endY = Mathf.Min(maxY, gridSize / 2);

        foreach (var prefabPos in spawnedPrefabs.Keys.ToList())
        {
            GameObject prefabInstance;
            if (spawnedPrefabs.TryGetValue(prefabPos, out prefabInstance))
            {
                if (prefabPos.x < startX || prefabPos.x >= endX || prefabPos.y < startY || prefabPos.y >= endY)
                {
                    prefabInstance.SetActive(false);
                }
                else
                {
                    prefabInstance.SetActive(true);
                }
            }
        }
    }

    private GameObject GetRandomPrefab()
    {
        float totalDensity = 0f;
        foreach (var data in prefabData)
        {
            totalDensity += data.density;
        }

        float randomDensity = Random.Range(0f, totalDensity);

        float densitySum = 0f;
        foreach (var data in prefabData)
        {
            densitySum += data.density;
            if (randomDensity <= densitySum)
            {
                return data.prefab;
            }
        }

        return null;
    }

    private bool ShouldSpawnPrefab(GameObject prefab, Vector2Int tilePos)
    {
        if (prefab == null)
            return false;

        float density = GetPrefabDensity(prefab);
        float randomValue = Random.Range(0f, 1f);

        if (randomValue > density)
            return false;

        float minDistance = GetPrefabMinDistance(prefab);

        foreach (var spawnedPrefabPos in spawnedPrefabs.Keys)
        {
            float distance = Vector2Int.Distance(spawnedPrefabPos, tilePos);
            if (distance < minDistance)
                return false;
        }

        return true;
    }

    private float GetPrefabDensity(GameObject prefab)
    {
        foreach (var data in prefabData)
        {
            if (data.prefab == prefab)
                return data.density;
        }

        return 0f;
    }

    private float GetPrefabMinDistance(GameObject prefab)
    {
        foreach (var data in prefabData)
        {
            if (data.prefab == prefab)
                return data.minDistance;
        }

        return 0f;
    }

    private Vector2Int GetTilePosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);

        return new Vector2Int(x, y);
    }
}