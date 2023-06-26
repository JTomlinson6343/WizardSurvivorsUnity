using UnityEngine;
using System.Collections.Generic;

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
                    spawnedPrefabs.Add(tilePos, prefabInstance);
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
