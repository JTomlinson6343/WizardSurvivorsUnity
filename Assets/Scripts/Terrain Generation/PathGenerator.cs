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

    private Transform startPoint;
    private Transform endPoint;

    public float minDistance = 10f;

    private void Start()
    {
        PlaceEndPoints();
        GeneratePath();
    }

    private void PlaceEndPoints()
    {
        float halfGridSize = gridSize / 2f;

        startPoint = Instantiate(pathEndPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(pathEndPointPrefab, Vector3.zero, Quaternion.identity);

        startPoint.position = GetRandomPositionWithinGrid(halfGridSize, minDistance);
        endPoint.position = GetRandomPositionWithinGrid(halfGridSize, minDistance);
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
        } while (Vector2.Distance(randomPosition, startPoint.position) < minDistanceFraction ||
                 Vector2.Distance(randomPosition, endPoint.position) < minDistanceFraction ||
                 !IsWithinGrid(randomPosition));

        return randomPosition;
    }

    private bool IsWithinGrid(Vector3 position)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(position);
        BoundsInt bounds = tilemap.cellBounds;
        return bounds.Contains(cellPosition);
    }

    private void GeneratePath()
    {
        Vector3Int startCell = tilemap.WorldToCell(startPoint.position);
        Vector3Int endCell = tilemap.WorldToCell(endPoint.position);

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
