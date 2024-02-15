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

    bool m_AttackLockout;
    [SerializeField] float m_AttackLockoutTime;

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
            if (GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange) && !m_AttackLockout)
            {
                ToggleWalkAnim(false);
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

    protected IEnumerator AttackLockout()
    {
        m_AttackLockout = true;

        yield return new WaitForSeconds(m_AttackLockoutTime);

        m_AttackLockout = false;
    }
    protected void FaceGameObject(Vector2 objectPos)
    {
        if (objectPos.x > transform.position.x)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();

        if (!particles) return;

        particles.transform.localScale = new Vector3(Mathf.Abs(particles.transform.localScale.x) * transform.localScale.x, particles.transform.localScale.y, particles.transform.localScale.z);
    }

    private void ToggleWalkAnim(bool toggle)
    {
        Animator animator = GetComponent<Animator>();
        if (!animator) return;

        animator.SetBool("Idle", !toggle);
        animator.SetBool("Walk", toggle);
    }

    virtual protected IEnumerator GoToPlayer()
    {
        m_TravellingToPlayer = true;
        ToggleWalkAnim(true);
        while (true)
        {
            if (!IsOutOfRange())
            {
                m_TravellingToPlayer = false;
                break;
            }
            transform.position = Vector2.MoveTowards(transform.position, Player.m_Instance.transform.position, m_MoveSpeed * Time.deltaTime);
            FaceGameObject(Player.m_Instance.transform.position);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return false;
    }

    virtual protected void GoToEnemy()
    {
        GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange * m_PursueEnemyRangeModifier);

        if (!enemy) return;
        ToggleWalkAnim(true);

        transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, m_MoveSpeed * Time.deltaTime);
        FaceGameObject(enemy.transform.position);
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
}
