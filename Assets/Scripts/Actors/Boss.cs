using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public string m_BossName;

    [SerializeField] int m_MaxSkillPoints;
    [SerializeField] int m_MinSkillPoints;

    private readonly float kGracePeriodTime = 5f;

    protected override void OnDeath()
    {
        base.OnDeath();
        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, Random.Range(m_MinSkillPoints, m_MaxSkillPoints + 1));
        ProgressionManager.m_Instance.RemoveBossBar();
        StateManager.ChangeState(State.PLAYING);
        EnemyManager.m_Instance.GracePeriod(kGracePeriodTime);
    }
}
