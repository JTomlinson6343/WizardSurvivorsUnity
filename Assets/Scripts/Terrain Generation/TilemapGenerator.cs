using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[System.Serializable]
public class TileData
{
    public TileBase tile;
    public int ratio;
}

public class TilemapGenerator : MonoBehaviour
{
    public TileData[] tileData;
    public int gridSize = 128; // Size of the grid and the overall map

    private Tilemap tilemap;
    private Dictionary<Vector2Int, TileData> spawnedTiles;
    private Transform playerTransform;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        spawnedTiles = new Dictionary<Vector2Int, TileData>();
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
                TileData selectedTile = GetRandomTile();

                if (selectedTile != null)
                {
                    spawnedTiles.Add(tilePos, selectedTile);
                    tilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), selectedTile.tile);
                }
            }
        }
    }

    private void Update()
    {
        // Your spawning logic or any other game-related updates can go here
    }

    private TileData GetRandomTile()
    {
        int totalRatios = 0;
        foreach (var data in tileData)
        {
            totalRatios += data.ratio;
        }

        int randomNumber = Random.Range(1, totalRatios + 1);

        int ratioSum = 0;
        foreach (var data in tileData)
        {
            ratioSum += data.ratio;
            if (randomNumber <= ratioSum)
            {
                return data;
            }
        }

        return null;
    }

    private Vector2Int GetTilePosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);

        return new Vector2Int(x, y);
    }
}
