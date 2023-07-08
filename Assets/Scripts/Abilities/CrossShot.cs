using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Ability
{
    [SerializeField] float m_Lifetime;
    public override void OnCast()
    {
        ProjectileManager.m_Instance.MultiShot(
            Player.m_Instance.GetStaffTransform().position, m_TotalStats.speed,
            m_TotalStats.amount, this, m_Lifetime);
    }
}
