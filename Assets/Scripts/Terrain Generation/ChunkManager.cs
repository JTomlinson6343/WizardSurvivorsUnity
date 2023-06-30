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
            Vector2 position = tilePrefab.transform.position;
            Quaternion rotation = tilePrefab.transform.rotation;

            GameObject tileInstance = Instantiate(tilePrefab, position, rotation);
            activeTiles.Add(new TileInstanceData(tileInstance, position));
        }
    }

    private void RemoveTilesOutsideView()
    {
        float viewDistance = GetViewDistance();

        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            TileInstanceData tileData = activeTiles[i];
            Vector2 position = tileData.position;

            if (IsPositionOutsideView(position, viewDistance))
            {
                Destroy(tileData.tile);
                activeTiles.RemoveAt(i);
            }
        }
    }

    private void AddTilesInsideView()
    {
        Vector2 playerPosition = playerCharacter.position;
        float viewDistance = GetViewDistance();

        foreach (GameObject tilePrefab in tiles)
        {
            Vector2 position = tilePrefab.transform.position;

            if (IsPositionInsideView(position, viewDistance))
            {
                bool found = false;

                foreach (TileInstanceData tileData in activeTiles)
                {
                    if (tileData.position == position)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Quaternion rotation = tilePrefab.transform.rotation;

                    GameObject tileInstance = Instantiate(tilePrefab, position, rotation);
                    activeTiles.Add(new TileInstanceData(tileInstance, position));
                }
            }
        }
    }

    private bool IsPositionOutsideView(Vector2 position, float viewDistance)
    {
        Vector2 playerPosition = playerCharacter.position;
        return Vector2.Distance(playerPosition, position) > viewDistance;
    }

    private bool IsPositionInsideView(Vector2 position, float viewDistance)
    {
        Vector2 playerPosition = playerCharacter.position;
        return Vector2.Distance(playerPosition, position) <= viewDistance;
    }

    private float GetViewDistance()
    {
        float baseDistance = GetBaseViewDistance();
        return baseDistance * renderDistanceModifier;
    }

    private float GetBaseViewDistance()
    {
        // Calculate the base view distance based on your desired logic
        // For example, you can use the scale of your tiles or a fixed value
        return 10f;
    }

    private class TileInstanceData
    {
        public GameObject tile;
        public Vector2 position;

        public TileInstanceData(GameObject tile, Vector2 position)
        {
            this.tile = tile;
            this.position = position;
        }
    }
}
