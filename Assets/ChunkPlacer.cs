using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ChunkPlacer : EditorWindow
{
    [SerializeField]
    private List<GameObject> chunkPrefabs = new List<GameObject>(); // List of prefabs with 16x16 tilemaps

    private const int chunkSize = 16;
    private const int worldSize = 256;

    [MenuItem("Window/Chunk Placer")]
    public static void ShowWindow()
    {
        GetWindow<ChunkPlacer>("Chunk Placer");
    }

    void OnGUI()
    {
        GUILayout.Label("Chunk Placement Settings", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);
        DrawPrefabList();

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(chunkPrefabs == null || chunkPrefabs.Count == 0);
        if (GUILayout.Button("Place Chunks"))
        {
            PlaceChunks();
        }
        EditorGUI.EndDisabledGroup();
    }

    void PlaceChunks()
    {
        if (chunkPrefabs == null || chunkPrefabs.Count == 0)
        {
            Debug.LogWarning("No chunk prefabs found. Please add prefabs with 16x16 tilemaps.");
            return;
        }

        int chunkCount = worldSize / chunkSize;

        int centerOffset = (chunkCount - 1) / 2; // Offset to center the grid

        for (int x = 0; x < chunkCount; x++)
        {
            for (int y = 0; y < chunkCount; y++)
            {
                // Calculate the position of the chunk, centered around (0, 0, 0)
                Vector3 position = new Vector3((x - centerOffset) * chunkSize, (y - centerOffset) * chunkSize, 0);

                // Select a random chunk prefab from the list
                GameObject chunkPrefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];

                // Instantiate the chunk prefab at the calculated position
                GameObject chunk = Instantiate(chunkPrefab, position, Quaternion.identity);
                chunk.name = $"Chunk(X:{position.x})(Y:{position.y})";
            }
        }
    }

    void DrawPrefabList()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        for (int i = 0; i < chunkPrefabs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            chunkPrefabs[i] = (GameObject)EditorGUILayout.ObjectField(chunkPrefabs[i], typeof(GameObject), false);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                chunkPrefabs.RemoveAt(i);
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Prefab"))
        {
            chunkPrefabs.Add(null);
        }

        EditorGUILayout.EndVertical();
    }
}
