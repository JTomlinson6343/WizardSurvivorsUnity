using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : Spell
{
    public float m_ProjectileLifetime;
    [SerializeField] protected GameObject m_BulletPrefab;


    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            aimDirection,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BulletPrefab);

        CastSound();
    }

    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy && m_DefaultAutofireRange >= 0)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        FireShot(Player.m_Instance.GetStaffTransform().position, closestEnemy);
    }

    public void FireShot(Vector2 pos, GameObject enemy)
    {
        if (!enemy) return;

        Vector2 dir = Utils.GetDirectionToGameObject(pos, enemy);

        ProjectileManager.m_Instance.Shoot(pos,
            dir,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BulletPrefab);

        CastSound();
    }
}
