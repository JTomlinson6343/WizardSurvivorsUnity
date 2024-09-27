using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    protected bool m_DidDamagePlayer = false;

    public string m_BossName;

    [SerializeField] int m_MinSkillPoints;

    [SerializeField] int m_MaxSkillPoints;

    private readonly float m_PercentHealthHealOnKill = 0.5f;

    // Modify the boss' stats based on when the boss is fought. 1 = first time a boss is fought
    abstract public void Enraged(int bossNumber);

    public virtual void BossFightInit()
    {
        PlayerManager.m_Instance.m_ActorsToBind = new Actor[] { Player.m_Instance, this };
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;

        m_DidDamagePlayer = true;
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, Random.Range(m_MinSkillPoints, m_MaxSkillPoints + 1));

        ProgressionManager.m_Instance.OnBossFightEnd();

        Player.m_Instance.PercentHeal(m_PercentHealthHealOnKill);
    }

    protected override void NormalDeath()
    {
        DeathParticlesRoutine();
    }
}
