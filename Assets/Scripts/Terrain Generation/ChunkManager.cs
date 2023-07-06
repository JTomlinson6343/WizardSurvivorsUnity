using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Transform playerCharacter;
    public float renderDistanceModifier = 1.0f;
    public GameObject[] tiles;

    private List<TileInstanceData> activeTiles = new List<TileInstanceData>();

    private void Start()
    {
        GenerateTiles();
    }

    private void Update()
    {
        RemoveTilesOutsideView();
        AddTilesInsideView();
    }

    private void GenerateTiles()
    {
        foreach (GameObject tilePrefab in tiles)
        {
            Vector2Int chunkPosition = new Vector2Int(
                Mathf.FloorToInt(tilePrefab.transform.position.x / 16),
                Mathf.FloorToInt(tilePrefab.transform.position.y / 16)
            );

            Vector2 position = new Vector2(
                chunkPosition.x * 16 + tilePrefab.transform.localPosition.x,
                chunkPosition.y * 16 + tilePrefab.transform.localPosition.y
            );

            Quaternion rotation = tilePrefab.transform.rotation;

            GameObject tileInstance = Instantiate(tilePrefab, position, rotation);
            activeTiles.Add(new TileInstanceData(tileInstance, chunkPosition));
        }
    }

    private void RemoveTilesOutsideView()
    {
        float viewDistance = GetViewDistance();

        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            TileInstanceData tileData = activeTiles[i];
            Vector2Int chunkPosition = tileData.chunkPosition;

            if (IsPositionOutsideView(chunkPosition, viewDistance))
            {
                Destroy(tileData.tile);
                activeTiles.RemoveAt(i);
            }
        }
    }

    private void AddTilesInsideView()
    {
        Vector2Int playerChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerCharacter.position.x / 16),
            Mathf.FloorToInt(playerCharacter.position.y / 16)
        );

        float viewDistance = GetViewDistance();

        foreach (GameObject tilePrefab in tiles)
        {
            Vector2Int chunkPosition = new Vector2Int(
                Mathf.FloorToInt(tilePrefab.transform.position.x / 16),
                Mathf.FloorToInt(tilePrefab.transform.position.y / 16)
            );

            if (IsPositionInsideView(chunkPosition, viewDistance))
            {
                bool found = false;

                foreach (TileInstanceData tileData in activeTiles)
                {
                    if (tileData.chunkPosition == chunkPosition)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Vector2 position = new Vector2(
                        chunkPosition.x * 16 + tilePrefab.transform.localPosition.x,
                        chunkPosition.y * 16 + tilePrefab.transform.localPosition.y
                    );

                    Quaternion rotation = tilePrefab.transform.rotation;

                    GameObject tileInstance = Instantiate(tilePrefab, position, rotation);
                    activeTiles.Add(new TileInstanceData(tileInstance, chunkPosition));
                }
            }
        }
    }

    private bool IsPositionOutsideView(Vector2Int chunkPosition, float viewDistance)
    {
        Vector2Int playerChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerCharacter.position.x / 16),
            Mathf.FloorToInt(playerCharacter.position.y / 16)
        );

        return Vector2Int.Distance(playerChunkPosition, chunkPosition) > viewDistance;
    }

    private bool IsPositionInsideView(Vector2Int chunkPosition, float viewDistance)
    {
        Vector2Int playerChunkPosition = new Vector2Int(
            Mathf.FloorToInt(playerCharacter.position.x / 16),
            Mathf.FloorToInt(playerCharacter.position.y / 16)
        );

        return Vector2Int.Distance(playerChunkPosition, chunkPosition) <= viewDistance;
    }

    private float GetViewDistance()
    {
        float baseDistance = GetBaseViewDistance();
        return baseDistance * renderDistanceModifier;
    }

    private float GetBaseViewDistance()
    {
        // Calculate the base view distance based on your desired logic
        // For example, you can use the scale of your chunks or a fixed value
        return 10f;
    }

    private class TileInstanceData
    {
        public GameObject tile;
        public Vector2Int chunkPosition;

        public TileInstanceData(GameObject tile, Vector2Int chunkPosition)
        {
            this.tile = tile;
            this.chunkPosition = chunkPosition;
        }
    }
}
