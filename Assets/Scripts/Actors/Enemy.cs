using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] float m_XPAwarded;

    private void Start()
    {
        m_ActorType = ActorType.Enemy;
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        ProgressionManager.m_Instance.AddXP(m_XPAwarded);
        ProgressionManager.m_Instance.AddScore(m_XPAwarded);
    }
}