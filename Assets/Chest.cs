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

    private enum ChestType
    {
        Wood
    }

    [SerializeField] ChestType m_Type;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_Opened) return;
        GetComponent<SpriteRenderer>().sprite = m_OpenSprite;
        m_Opened = true;
        SpawnLoot();
    }

    void SpawnLoot()
    {
        GameObject loot = Instantiate(GetComponent<LootTable>().GetLoot());

        int amount = Random.Range(m_MinDropAmount, m_MaxDropAmount);

        ProgressionManager.m_Instance.SpawnPickup(loot, transform.position, amount, new Vector2(0f, -1f));
    }
}
