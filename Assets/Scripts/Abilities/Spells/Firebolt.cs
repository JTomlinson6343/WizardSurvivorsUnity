using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : Spell
{
    [SerializeField] protected float m_ProjectileLifetime;
    [SerializeField] protected GameObject m_BulletPrefab;

    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            aimDirection,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BulletPrefab);

        AudioManager.m_Instance.PlaySound(4);
    }

    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        Vector2 dir = Utils.GetDirectionToGameObject(Player.m_Instance.GetStaffTransform().position, closestEnemy);

        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            dir,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BulletPrefab);

        AudioManager.m_Instance.PlaySound(4);
    }
}
