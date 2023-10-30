using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] float m_XPAwarded;
    public float m_ContactDamage;

    private void Start()
    {
        m_Health = m_MaxHealth;
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        ProgressionManager.m_Instance.AddXP(m_XPAwarded);
        ProgressionManager.m_Instance.AddScore(m_XPAwarded);
        ProgressionManager.m_Instance.IncrementEnemyKills();
    }
}