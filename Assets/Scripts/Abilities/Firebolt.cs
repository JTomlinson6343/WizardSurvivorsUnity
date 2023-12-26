using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : Ability
{
    [SerializeField] protected float m_ProjectileLifetime;
    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            Player.m_Instance.GetAimDirection().normalized,
            m_TotalStats.speed, this, m_ProjectileLifetime);

        AudioManager.m_Instance.PlaySound(4);
    }

    public override void OnCast()
    {
        base.OnCast();

        Vector2 closestEnemyPos = (Vector2)GameplayManager.GetClosestEnemyInRange(Player.m_Instance.GetCentrePos(), kDefaultAutofireRange).transform.position;

        if (closestEnemyPos == Vector2.negativeInfinity)
            return;

        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            closestEnemyPos,
            m_TotalStats.speed, this, m_ProjectileLifetime);

        AudioManager.m_Instance.PlaySound(4);
    }
}
