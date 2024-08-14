using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpawningProtectileSpell : Firebolt
{
    [SerializeField] GameObject m_AOEPrefab;

    public override void OnMouseInput(Vector2 aimDirection)
    {
        ProjectileManager.m_Instance.ShootAOESpawningProjectile(Player.m_Instance.GetStaffTransform().position,
            Player.m_Instance.GetMouseAimDirection().normalized,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_AOEPrefab, m_TotalStats.duration, m_BulletPrefab);

        PlaySound();
    }

    public override void OnCast()
    {
        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        ProjectileManager.m_Instance.ShootAOESpawningProjectile(Player.m_Instance.GetStaffTransform().position,
            Utils.GetDirectionToGameObject(Player.m_Instance.GetCentrePos(), closestEnemy),
            m_TotalStats.speed, this, m_ProjectileLifetime, m_AOEPrefab, m_TotalStats.duration, m_BulletPrefab);

        PlaySound();
    }

    virtual protected void PlaySound()
    {
        AudioManager.m_Instance.PlayRandomPitchSound(4);
    }
}