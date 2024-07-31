using System.Collections;
using UnityEngine;

public class PurpleGhostSpell : Firebolt
{
    public override void OnCast()
    {
        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        Vector2 dir = Utils.GetDirectionToGameObject(Player.m_Instance.GetStaffTransform().position, closestEnemy);

        GameObject bullet = ProjectileManager.m_Instance.Shoot(Player.m_Instance.GetStaffTransform().position,
            dir,
            m_TotalStats.speed, this, m_ProjectileLifetime, m_BulletPrefab);

        bullet.GetComponent<PurpleGhost>().m_CurrentTarget = closestEnemy;

        AudioManager.m_Instance.PlaySound(4);
    }
}