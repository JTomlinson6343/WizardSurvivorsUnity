using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Fireball
{
    [SerializeField] GameObject m_BlackHoleAOEPrefab;

    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.ShootAOESpawningProjectile(Player.m_Instance.GetStaffTransform().position,
            Player.m_Instance.GetAimDirection().normalized,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BlackHoleAOEPrefab, m_TotalStats.duration);

        AudioManager.m_Instance.PlaySound(4);
    }

    public override void OnCast()
    {
        base.OnCast();

        if (GameplayManager.GetClosestEnemyPos(Player.m_Instance.GetCentrePos()) == Vector2.negativeInfinity)
            return;

        ProjectileManager.m_Instance.ShootAOESpawningProjectile(Player.m_Instance.GetStaffTransform().position,
            GameplayManager.GetDirectionToEnemy(Player.m_Instance.GetCentrePos()),
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BlackHoleAOEPrefab, m_TotalStats.duration);

        AudioManager.m_Instance.PlaySound(4);
    }
}