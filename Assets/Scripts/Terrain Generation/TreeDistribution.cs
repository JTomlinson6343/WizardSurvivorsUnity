using UnityEngine;

[System.Serializable]
public struct TreePrefabData
{
    public GameObject prefab;
    public float density;
}

public class TreeDistribution : MonoBehaviour
{
    public TreePrefabData[] treeData; // Array of tree prefab data

    public int areaSize = 128; // Size of the distribution area
    public float spawnChance = 0.2f; // Probability of spawning a tree at each position

    void Start()
    {
        DistributeTrees();
    }

    void DistributeTrees()
    {
        foreach (TreePrefabData data in treeData)
        {
            float density = data.density;
            int numberOfTrees = Mathf.RoundToInt(density * areaSize * areaSize);

            for (int j = 0; j < numberOfTrees; j++)
            {
                if (Random.value <= spawnChance)
                {
                    Vector3 position = new Vector3(Random.Range(-areaSize / 2, areaSize / 2), Random.Range(-areaSize / 2, areaSize / 2), 0f);
                    GameObject newTree = Instantiate(data.prefab, position, Quaternion.identity);
                    newTree.transform.parent = transform;
                }
            }
        }
    }
}
