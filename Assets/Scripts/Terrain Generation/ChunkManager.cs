using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Transform playerCharacter;
    public float renderDistanceModifier = 1.0f;
    public GameObject[] chunks;
    public GameObject[] randomChunks;

    private List<TileInstanceData> activeChunks = new List<TileInstanceData>();

    private void Start()
    {
        GenerateChunks();
    }

    private void Update()
    {
        DisableChunksOutsideView();
        EnableChunksInsideView();
    }

    private void GenerateChunks()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            GameObject chunkPrefab = chunks[i];
            Vector2 position = chunkPrefab.transform.position;
            Quaternion rotation = chunkPrefab.transform.rotation;

            if (chunkPrefab.CompareTag("NonStaticChunk"))
            {
                int randomIndex = Random.Range(0, randomChunks.Length);
                GameObject randomChunkPrefab = randomChunks[randomIndex];
                GameObject chunkInstance = Instantiate(randomChunkPrefab, position, rotation, gameObject.transform);

                chunks[i] = chunkInstance; // Replace the element in the chunks array
                activeChunks.Add(new TileInstanceData(chunkInstance, position));
            }
            else
            {
                GameObject chunkInstance = Instantiate(chunkPrefab, position, rotation, gameObject.transform);
                activeChunks.Add(new TileInstanceData(chunkInstance, position));
            }
        }
    }

    private void DisableChunksOutsideView()
    {
        float viewDistance = GetViewDistance();

        foreach (TileInstanceData chunkData in activeChunks)
        {
            Vector2 position = chunkData.position;

            if (IsPositionOutsideView(position, viewDistance))
            {
                chunkData.chunk.SetActive(false); // Disable the chunk instead of destroying it
            }
            else
            {
                chunkData.chunk.SetActive(true); // Enable the chunk if it is within view range
            }
        }
    }

    private void EnableChunksInsideView()
    {
        Vector2 playerPosition = playerCharacter.position;
        float viewDistance = GetViewDistance();

        foreach (GameObject chunkPrefab in chunks)
        {
            Vector2 position = chunkPrefab.transform.position;

            if (IsPositionInsideView(position, viewDistance))
            {
                bool found = false;

                foreach (TileInstanceData chunkData in activeChunks)
                {
                    if (chunkData.position == position)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Quaternion rotation = chunkPrefab.transform.rotation;
                    GameObject chunkInstance = Instantiate(chunkPrefab, position, rotation);
                    activeChunks.Add(new TileInstanceData(chunkInstance, position));
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
        // For example, you can use the scale of your chunks or a fixed value
        return 10f;
    }

    private class TileInstanceData
    {
        public GameObject chunk;
        public Vector2 position;

        public TileInstanceData(GameObject chunk, Vector2 position)
        {
            this.chunk = chunk;
            this.position = position;
        }
    }
}
