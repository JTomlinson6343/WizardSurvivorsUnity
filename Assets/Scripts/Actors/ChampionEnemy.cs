using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionEnemy : Enemy
{
    [SerializeField] int m_MinSkillPoints;
    [SerializeField] int m_MaxSkillPoints;
    public override void Init()
    {
        base.Init();
        transform.localScale *= 3f;
        m_MaxHealth *= 2f;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    protected override void OnDeath()
    {
        Destroy(gameObject);
        ProgressionManager.m_Instance.SpawnXP(transform.position, m_XPAwarded);
        ProgressionManager.m_Instance.IncrementEnemyKills();
        ProgressionManager.m_Instance.IncrementChampionKills();
        EnemySpawner.m_Instance.IncrementEnemiesKilled();
        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, Random.Range(m_MinSkillPoints, m_MaxSkillPoints));
    }
}