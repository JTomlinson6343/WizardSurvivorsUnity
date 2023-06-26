using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathGenerator : MonoBehaviour
{
    public Transform pathEndPointPrefab;
    public Tilemap tilemap;
    public TileBase[] tileArray;
    public int gridSize = 128;
    public int pathWidth = 2;
    public GameObject player;
    public float minDistance = 10f;

    private Transform startPoint;
    private Transform endPoint;

    private void Start()
    {
        Vector3 playerPosition = player.transform.position;
        PlaceEndPoints(playerPosition);
        GeneratePath();
    }

    private void PlaceEndPoints(Vector3 playerPosition)
    {
        float halfGridSize = gridSize / 2f;

        startPoint = Instantiate(pathEndPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(pathEndPointPrefab, Vector3.zero, Quaternion.identity);

        Vector3 randomOffset;
        float distance;
        do
        {
            randomOffset = GetRandomPositionWithinGrid(halfGridSize, minDistance);
            distance = Vector3.Distance(playerPosition + randomOffset, playerPosition);
        } while (distance < minDistance);

        startPoint.position = playerPosition + randomOffset;
        endPoint.position = playerPosition - randomOffset;
    }

    private Vector3 GetRandomPositionWithinGrid(float halfGridSize, float minDistanceFraction)
    {
        Vector3 randomPosition;
        do
        {
            randomPosition = new Vector3(
                Random.Range(-halfGridSize + 1f, halfGridSize - 1f),
                Random.Range(-halfGridSize + 1f, halfGridSize - 1f),
                0f);

            // Clamp the random position within the grid boundaries
            randomPosition.x = Mathf.Clamp(randomPosition.x, -halfGridSize + minDistanceFraction, halfGridSize - minDistanceFraction);
            randomPosition.y = Mathf.Clamp(randomPosition.y, -halfGridSize + minDistanceFraction, halfGridSize - minDistanceFraction);

        } while (Vector2.Distance(randomPosition, startPoint.position) < minDistanceFraction ||
                 Vector2.Distance(randomPosition, endPoint.position) < minDistanceFraction);

        return randomPosition;
    }

    private void GeneratePath()
    {
        Vector3Int startCell = tilemap.WorldToCell(startPoint.position);
        Vector3Int endCell = tilemap.WorldToCell(endPoint.position);

        if (startCell == endCell)
        {
            Debug.LogWarning("Start and end cells are the same. Skipping path generation.");
            return;
        }

        Vector3Int[] pathCells = CalculatePathCells(startCell, endCell);

        foreach (Vector3Int cell in pathCells)
        {
            for (int i = -pathWidth / 2; i <= pathWidth / 2; i++)
            {
                for (int j = -pathWidth / 2; j <= pathWidth / 2; j++)
                {
                    Vector3Int offset = new Vector3Int(i, j, 0);
                    tilemap.SetTile(cell + offset, GetRandomTile());
                }
            }
        }
    }

    private Vector3Int[] CalculatePathCells(Vector3Int startCell, Vector3Int endCell)
    {
        List<Vector3Int> pathCells = new List<Vector3Int>();

        // Calculate the horizontal distance between the start and end points
        int distanceX = endCell.x - startCell.x;

        // Calculate the vertical distance between the start and end points
        int distanceY = endCell.y - startCell.y;

        // Determine the direction of movement along the x-axis (left or right)
        int stepX = (distanceX > 0) ? 1 : -1;

        // Determine the direction of movement along the y-axis (up or down)
        int stepY = (distanceY > 0) ? 1 : -1;

        // Calculate the absolute distances
        int absDistanceX = Mathf.Abs(distanceX);
        int absDistanceY = Mathf.Abs(distanceY);

        // Determine the total number of steps required
        int totalSteps = Mathf.Max(absDistanceX, absDistanceY);

        // Calculate the cell increment along the x-axis and y-axis
        float cellIncrementX = (float)distanceX / totalSteps;
        float cellIncrementY = (float)distanceY / totalSteps;

        // Generate the path by adding cells to the pathCells list
        Vector3Int currentCell = startCell;
        for (int i = 0; i <= totalSteps; i++)
        {
            pathCells.Add(currentCell);

            // Add random offsets to the cell position
            float randomOffsetX = Random.Range(-1f, 1f) * cellIncrementX;
            float randomOffsetY = Random.Range(-1f, 1f) * cellIncrementY;

            currentCell.x += stepX;
            currentCell.y += stepY;

            // Apply the random offsets
            currentCell.x += Mathf.RoundToInt(randomOffsetX);
            currentCell.y += Mathf.RoundToInt(randomOffsetY);
        }

        return pathCells.ToArray();
    }

    private TileBase GetRandomTile()
    {
        int randomIndex = Random.Range(0, tileArray.Length);
        return tileArray[randomIndex];
    }
}
