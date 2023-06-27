using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] float m_XPAwarded;

    protected override void OnDeath()
    {
        base.OnDeath();

        ProgressionManager.m_Instance.AddXP(m_XPAwarded);
        ProgressionManager.m_Instance.AddScore(m_XPAwarded);
    }
}