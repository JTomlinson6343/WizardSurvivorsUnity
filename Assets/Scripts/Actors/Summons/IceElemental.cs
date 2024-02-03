using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceElemental : Summon
{
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileCooldown;
    bool m_IsProjectileOnCooldown;

    [SerializeField] int m_ShotsInBurst;
    [SerializeField] float m_ShotDelay;
    override protected void Attack()
    {
        if (m_IsProjectileOnCooldown) return;

        GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange);
        if (!enemy) return;

        StartCoroutine(MultiShotBurst(enemy));

        m_IsProjectileOnCooldown = true;
        Invoke(nameof(EndShotCooldown), m_ProjectileCooldown);
    }

    IEnumerator MultiShotBurst(GameObject target)
    {
        for (int i = 0; i < m_ShotsInBurst; i++)
        {
            if (!target) yield break;

            ProjectileManager.m_Instance.Shoot(
                transform.position,
                GameplayManager.GetDirectionToGameObject(transform.position, target),
                m_AbilitySource.GetTotalStats().speed,
                m_AbilitySource,
                m_ProjectileLifetime
                );
            yield return new WaitForSeconds(m_ShotDelay);
        }
    }

    private void EndShotCooldown()
    {
        m_IsProjectileOnCooldown = false;
    }
}
