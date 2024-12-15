using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite m_OpenSprite;
    [SerializeField] int m_MinDropAmount;
    [SerializeField] int m_MaxDropAmount;
    bool m_Opened = false;


    [SerializeField] string m_LootTableName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_Opened || !collision.gameObject.CompareTag("Player")) return;
        GetComponent<SpriteRenderer>().sprite = m_OpenSprite;
        m_Opened = true;

        SpawnLoot();
        AudioManager.m_Instance.PlayRandomPitchSound(34);
    }

    void SpawnLoot()
    {
        int amount = Random.Range(m_MinDropAmount, m_MaxDropAmount);

        float modifier = 1f + amount / 10f;

        for (int i = 0; i < amount; i++)
        {
            GameObject loot = LootManager.GetLootTableWithName("BasicChest").GetLoot();

            ProgressionManager.m_Instance.SpawnPickup(loot, transform.position, 1, new Vector2(0f, -1f * modifier));
        }
    }
}