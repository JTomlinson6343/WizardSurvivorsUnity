using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSpell : Spell
{
    [SerializeField] float m_Range;
    [SerializeField] GameObject m_IcePrefab;

    public override void OnCast()
    {
        base.OnCast();

        GameObject enemy = GetRandomEnemy();

        if (!enemy) return;

        GameObject ice = Instantiate(m_IcePrefab);

        ice.GetComponent<AOEObject>().Init(enemy.transform.position, this, GetTotalStats().duration);
    }

    private GameObject GetRandomEnemy()
    {
        List<GameObject> enemies = GameplayManager.GetAllEnemiesInRange(Player.m_Instance.transform.position, m_Range);

        if (enemies.Count <= 0) return null;

        int RandNo = Random.Range(0, enemies.Count);

        return enemies[RandNo];
    }
}
