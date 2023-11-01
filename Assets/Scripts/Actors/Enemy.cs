using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] int m_XPAwarded;
    public float m_ContactDamage;

    override public void Start()
    {
        base.Start();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        ProgressionManager.m_Instance.AddXP(m_XPAwarded);
        ProgressionManager.m_Instance.AddScore(m_XPAwarded);
        ProgressionManager.m_Instance.IncrementEnemyKills();
        EnemySpawner.m_Instance.IncrementEnemiesKilled();
    }
}