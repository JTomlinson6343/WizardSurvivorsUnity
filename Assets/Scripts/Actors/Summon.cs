using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_LeashRange;
    [SerializeField] float m_PursueEnemyRangeModifier;
    [SerializeField] float m_LeashCheckDelay;
    float m_LastLeashCheck;
    bool m_TravellingToPlayer;

    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileCooldown;
    bool  m_IsProjectileOnCooldown;

    public Ability m_AbilitySource;

    [SerializeField] GameObject m_FlamethrowerObject;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector2.zero, m_LeashRange);
    }

    private void Start()
    {
        m_FlamethrowerObject.GetComponentInChildren<DebuffAOE>().m_AbilitySource = m_AbilitySource;

    }

    private void Update()
    {
        Brain();
    }

    private void Brain()
    {
        if (!m_TravellingToPlayer)
        {
            if (GameplayManager.GetClosestEnemyInRange(transform.position, 4.25f))
            {
                //Attack();
                ShootFlames();
            }
            else if (!m_IsProjectileOnCooldown)
            {
                DisableFlames();
                GoToEnemy();
            }
        }

        if (IsOutOfRangeTooLong())
        {
            DisableFlames();
            StartCoroutine(GoToPlayer());
        }
    }

    IEnumerator GoToPlayer()
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

    private void GoToEnemy()
    {
        GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, 4.25f * m_PursueEnemyRangeModifier);

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

    private void Attack()
    {
        if (m_IsProjectileOnCooldown) return;

        GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.GetTotalStats().AOE);
        if (!enemy)
        {
            print("Enemy null");
            return;
        }
        print("enemy not null");

        ProjectileManager.m_Instance.Shoot(
            transform.position,
            GameplayManager.GetDirectionToGameObject(transform.position, enemy),
            m_AbilitySource.GetTotalStats().speed,
            m_AbilitySource,
            m_ProjectileLifetime
            );

        m_IsProjectileOnCooldown = true;
        Invoke(nameof(EndShotCooldown), m_ProjectileCooldown);
    }

    private void ShootFlames()
    {
        GameObject closestEnemy = GameplayManager.GetClosestEnemyInRange(transform.position, 4.25f);

        if (!closestEnemy)
        {
            DisableFlames();
            return;
        }

        m_FlamethrowerObject.SetActive(true);

        Vector2 dir = GameplayManager.GetDirectionToGameObject(transform.position, closestEnemy);

        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }

    private void DisableFlames()
    {
        m_FlamethrowerObject.SetActive(false);
    }

    void EndShotCooldown()
    {
        m_IsProjectileOnCooldown = false;
    }
}
