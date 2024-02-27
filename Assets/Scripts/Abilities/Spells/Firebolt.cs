using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : Spell
{
    [SerializeField] protected float m_ProjectileLifetime;

    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            aimDirection,
            m_TotalStats.speed, this, m_ProjectileLifetime);

        AudioManager.m_Instance.PlaySound(4);
    }

    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = GameplayManager.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            ResetCooldown(kCooldownAfterReset);
            return;
        }

        Vector2 dir = GameplayManager.GetDirectionToGameObject(Player.m_Instance.GetStaffTransform().position, closestEnemy);

        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            dir,
            m_TotalStats.speed, this, m_ProjectileLifetime);

        AudioManager.m_Instance.PlaySound(4);
    }
}
