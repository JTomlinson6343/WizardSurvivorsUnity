using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Firebolt
{
    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] GameObject m_AOEPrefab;

    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.ShootAOESpawningProjectile(Player.m_Instance.GetStaffTransform().position,
            Player.m_Instance.GetAimDirection().normalized,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_AOEPrefab, m_TotalStats.duration, m_ProjectilePrefab);

        AudioManager.m_Instance.PlaySound(4);
    }

    public override void OnCast()
    {
        GameObject closestEnemy = GameplayManager.GetClosestEnemyInRange(Player.m_Instance.GetCentrePos(), kDefaultAutofireRange);

        if (!closestEnemy) return;

        ProjectileManager.m_Instance.ShootAOESpawningProjectile(Player.m_Instance.GetStaffTransform().position,
            GameplayManager.GetDirectionToEnemy(Player.m_Instance.GetCentrePos(), closestEnemy),
            m_TotalStats.speed, this, m_ProjectileLifetime, m_AOEPrefab, m_TotalStats.duration, m_ProjectilePrefab);

        AudioManager.m_Instance.PlaySound(4);
    }
}