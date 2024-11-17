using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceElemental : Summon
{
    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileCooldown;
    bool m_IsProjectileOnCooldown;

    [SerializeField] int m_ShotsInBurst;
    [SerializeField] float m_ShotDelay;
    [SerializeField] GameObject m_Staff;
    override protected void Attack()
    {
        if (m_IsProjectileOnCooldown) return;

        GameObject enemy = Utils.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange);
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
                m_Staff.transform.position,
                Utils.GetDirectionToGameObject(m_Staff.transform.position, target),
                m_AbilitySource.GetTotalStats().speed,
                m_AbilitySource,
                m_ProjectileLifetime,
                m_ProjectilePrefab
                );

            AudioManager.m_Instance.PlayRandomPitchSound(19, 0.4f);

            yield return new WaitForSeconds(m_ShotDelay);
        }
    }

    private void EndShotCooldown()
    {
        m_IsProjectileOnCooldown = false;
    }
}
