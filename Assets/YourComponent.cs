using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class YourComponent : MonoBehaviour
{
    public GameObject[] trees;
    public int numTrees;
    public GameObject[] bushes;
    public int numBushes;
    public GameObject[] grass;
    public int numGrass;
    public TileBase[] tiles;
    public Transform groundTilemap;

    public void Generate()
    {
        // Clear the tilemap
        Tilemap tilemap = groundTilemap.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();

        // Define the size of the grid
        int gridSize = 15;

        // Calculate the half size of the grid
        int halfSize = gridSize / 2;

        // Create a list to store available tile positions
        List<Vector3Int> availablePositions = new List<Vector3Int>();

        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                // Add the current tile position to the available positions list
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                availablePositions.Add(tilePosition);

                // Get a random tile from the array
                int randomIndex = Random.Range(0, tiles.Length);
                TileBase randomTile = tiles[randomIndex];

                // Set the random tile at the current position in the tilemap
                tilemap.SetTile(tilePosition, randomTile);
            }
        }

        // Randomly distribute prefabs from each array
        DistributePrefabs(trees, numTrees, availablePositions);
        DistributePrefabs(bushes, numBushes, availablePositions);
        DistributePrefabs(grass, numGrass, availablePositions);
    }

    private void DistributePrefabs(GameObject[] prefabArray, int numEntities, List<Vector3Int> availablePositions)
    {
        for (int i = 0; i < numEntities; i++)
        {
            // Get a random prefab from the array
            int randomIndex = Random.Range(0, prefabArray.Length);
            GameObject randomPrefab = prefabArray[randomIndex];

            // Get a random available position from the list
            int randomPositionIndex = Random.Range(0, availablePositions.Count);
            Vector3Int randomPosition = availablePositions[randomPositionIndex];

            // Remove the selected position from the available positions list
            availablePositions.RemoveAt(randomPositionIndex);

            // Instantiate the prefab at the selected position
            Instantiate(randomPrefab, randomPosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f), Quaternion.identity, groundTilemap);
        }
    }
}
