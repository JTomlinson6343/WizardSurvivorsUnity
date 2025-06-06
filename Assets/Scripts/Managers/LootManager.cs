using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager m_Instance;

    public List<LootTable> m_LootTables;

    public static LootTable GetLootTableWithName(string name) { return m_Instance.m_LootTables.Find(x => x.name == name); }

    private void Awake()
    {
        m_Instance = this;
    }
}

[System.Serializable]
public struct LootTable
{
    public string name;
    [System.Serializable]
    private struct Loot
    {
        public GameObject loot;
        public float probability;
    }

    [SerializeField] Loot[] m_LootTable;

    public GameObject GetLoot()
    {
        GameObject lootToSpawn = null;
        // Generate a random number between 0 and the total probability
        float randomValue = Random.Range(0f, CalculateSpawnProbability());

        // Select the enemy based on the generated random value
        foreach (Loot loot in m_LootTable)
        {
            if (randomValue < loot.probability)
            {
                lootToSpawn = loot.loot;
                break;
            }
            randomValue -= loot.probability;
        }
        return lootToSpawn;
    }

    private float CalculateSpawnProbability()
    {
        float totalProbability = 0f;
        // Calculate the total spawn probability
        foreach (Loot loot in m_LootTable)
        {
            totalProbability += loot.probability;
        }
        return totalProbability;
    }
}
