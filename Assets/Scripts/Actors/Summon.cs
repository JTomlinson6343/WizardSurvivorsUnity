using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Summon : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_LeashRange;
    [SerializeField] float m_PursueEnemyRangeModifier;
    [SerializeField] float m_LeashCheckDelay;
    float m_LastLeashCheck;
    bool m_TravellingToPlayer;

    public Ability m_AbilitySource;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector2.zero, m_LeashRange);
    }

    private void Update()
    {
        Brain();
    }

    private void Brain()
    {
        if (!m_TravellingToPlayer)
        {
            if (GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange))
            {
                Attack();
            }
            else
            {
                GoToEnemy();
            }
        }

        if (IsOutOfRangeTooLong())
        {
            StartCoroutine(GoToPlayer());
        }
    }

    virtual protected IEnumerator GoToPlayer()
    {
        m_TravellingToPlayer = true;
        while (true)
        {
            if (!IsOutOfRange())
            {
                m_TravellingToPlayer = false;
                break;
            }
            transform.position = Vector2.MoveTowards(transform.position, Player.m_Instance.transform.position, m_MoveSpeed * Time.deltaTime);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return false;
    }

    virtual protected void GoToEnemy()
    {
        GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange * m_PursueEnemyRangeModifier);

        if (!enemy) return;

        transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, m_MoveSpeed * Time.deltaTime);
    }

    private bool IsOutOfRange()
    {
        return Vector2.Distance(transform.position, Player.m_Instance.transform.position) > m_LeashRange;
    }

    // Check if summon been out of range of player for too long
    private bool IsOutOfRangeTooLong()
    {
        float now = Time.realtimeSinceStartup;

        if (!IsOutOfRange())
        {
            m_LastLeashCheck = now;
            return false;
        }

        if (now - m_LastLeashCheck < m_LeashCheckDelay)
        {
            return false;
        }

        m_LastLeashCheck = now;
        return true;
    }

    protected abstract void Attack();
    //private void Attack()
    //{
    //    if (m_IsProjectileOnCooldown) return;

    //    GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.GetTotalStats().AOE);
    //    if (!enemy)
    //    {
    //        print("Enemy null");
    //        return;
    //    }
    //    print("enemy not null");

    //    ProjectileManager.m_Instance.Shoot(
    //        transform.position,
    //        GameplayManager.GetDirectionToGameObject(transform.position, enemy),
    //        m_AbilitySource.GetTotalStats().speed,
    //        m_AbilitySource,
    //        m_ProjectileLifetime
    //        );

    //    m_IsProjectileOnCooldown = true;
    //    Invoke(nameof(EndShotCooldown), m_ProjectileCooldown);
    //}
}
