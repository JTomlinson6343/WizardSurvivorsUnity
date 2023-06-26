using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Ability
{
    [SerializeField] Color m_Colour;
    [SerializeField] float m_Lifetime;
    public override void OnCast()
    {
        ProjectileManager.m_Instance.MultiShot(
            Player.m_Instance.GetStaffTransform().position, m_TotalStats.speed, m_Colour,
            m_TotalStats.amount, m_TotalStats.damageScaling, m_Lifetime);
    }
}
