using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpawningSpell : Spell
{
    [SerializeField] protected float m_Range;
    [SerializeField] protected GameObject m_AOEPrefab;

    public override void OnCast()
    {
        base.OnCast();

        GameObject enemy = GetRandomEnemy();

        if (!enemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        GameObject aoe = Instantiate(m_AOEPrefab);

        aoe.GetComponent<AOEObject>().Init(enemy.transform.position, this, GetTotalStats().duration);

        PlaySound();
    }

    protected GameObject GetRandomEnemy()
    {
        List<GameObject> enemies = Utils.GetAllEnemiesInRange(Player.m_Instance.transform.position, m_Range);

        if (enemies.Count <= 0) return null;

        int RandNo = Random.Range(0, enemies.Count);

        return enemies[RandNo];
    }

    protected virtual void PlaySound()
    {
        CastSound();
    }
}
