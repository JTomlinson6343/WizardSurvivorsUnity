using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    [SerializeField] float m_Lifetime;
    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            Player.m_Instance.GetAimDirection().normalized,
            m_TotalStats.speed, this, m_Lifetime);
    }

    public override void OnCast()
    {
        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            Player.m_Instance.GetAimDirection().normalized,
            m_TotalStats.speed, this, m_Lifetime);
    }
}
