using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Spell
{
    [SerializeField] float m_Lifetime;
    public override void OnCast()
    {
        if (StateManager.IsGameplayStopped()) { return; }

        base.OnCast();

        ProjectileManager.m_Instance.MultiShot(
            Player.m_Instance.GetStaffTransform().position, m_TotalStats.speed,
            m_TotalStats.castAmount, this, m_Lifetime);

        CastSound();
    }
}
