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
    public float renderDistance = 20f;
    public int gridSize = 128; // Size of the grid and the overall map

    private Tilemap tilemap;
    private Dictionary<Vector2Int, TileData> spawnedTiles;
    private Transform playerTransform;
    private Vector3Int[] tilePositionsBuffer;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        spawnedTiles = new Dictionary<Vector2Int, TileData>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        tilePositionsBuffer = new Vector3Int[gridSize * gridSize];
    }

    private void Start()
    {
        GenerateInitialGrid();
    }

    private void Update()
    {
        RenderTilesInRange();
    }

    private void GenerateInitialGrid()
    {
        Vector2Int playerTile = GetTilePosition(playerTransform.position);
        int halfGridSize = gridSize / 2;

        int startX = playerTile.x - halfGridSize;
        int startY = playerTile.y - halfGridSize;
        int endX = playerTile.x + halfGridSize;
        int endY = playerTile.y + halfGridSize;

        int tilePositionsIndex = 0;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);
                TileData selectedTile = GetRandomTile();

                if (selectedTile != null)
                {
                    spawnedTiles.Add(tilePos, selectedTile);
                    tilePositionsBuffer[tilePositionsIndex++] = new Vector3Int(tilePos.x, tilePos.y, 0);
                }
            }
        }

        tilemap.SetTiles(tilePositionsBuffer, GetTilesFromBuffer(tilePositionsIndex));
    }

    private void RenderTilesInRange()
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

        int tilePositionsIndex = 0;

        foreach (var tilePos in spawnedTiles.Keys)
        {
            if (tilePos.x < startX || tilePos.x >= endX || tilePos.y < startY || tilePos.y >= endY)
            {
                tilemap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), null);
            }
            else
            {
                TileData tileData;
                if (spawnedTiles.TryGetValue(tilePos, out tileData))
                {
                    tilePositionsBuffer[tilePositionsIndex++] = new Vector3Int(tilePos.x, tilePos.y, 0);
                }
            }
        }

        tilemap.SetTiles(tilePositionsBuffer, GetTilesFromBuffer(tilePositionsIndex));

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);

                if (!spawnedTiles.ContainsKey(tilePos))
                {
                    TileData selectedTile = GetRandomTile();
                    if (selectedTile != null)
                    {
                        spawnedTiles.Add(tilePos, selectedTile);
                        tilePositionsBuffer[tilePositionsIndex++] = new Vector3Int(tilePos.x, tilePos.y, 0);
                    }
                }
            }
        }

        tilemap.SetTiles(tilePositionsBuffer, GetTilesFromBuffer(tilePositionsIndex));
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

    private TileBase[] GetTilesFromBuffer(int count)
    {
        TileBase[] tiles = new TileBase[count];
        for (int i = 0; i < count; i++)
        {
            Vector3Int tilePos = tilePositionsBuffer[i];
            Vector2Int tilePosition = new Vector2Int(tilePos.x, tilePos.y);
            TileData tileData;
            if (spawnedTiles.TryGetValue(tilePosition, out tileData))
            {
                tiles[i] = tileData.tile;
            }
        }
        return tiles;
    }
}
