using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    public string m_BossName;

    [SerializeField] int m_MaxSkillPoints;
    [SerializeField] int m_MinSkillPoints;

    // Modify the boss' stats based on when the boss is fought. 1 = first time a boss is fought
    abstract public void Enraged(int bossNumber);

    protected override void OnDeath()
    {
        base.OnDeath();
        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, Random.Range(m_MinSkillPoints, m_MaxSkillPoints + 1));

        ProgressionManager.m_Instance.OnBossFightEnd();
    }
}
