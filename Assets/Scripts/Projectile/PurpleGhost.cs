using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleGhost : ConstantDamageAOE
{
    const float kBounceRadius = 4f;
    [SerializeField] int m_MaxBounces;
    int m_BounceCount = 0;
    public GameObject m_CurrentTarget;

    private bool m_InDeathAnim;

    public override void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        transform.position = pos;

        m_PierceCount = m_AbilitySource.GetTotalStats().pierceAmount;

        m_BounceCount -= ability.GetTotalStats().pierceAmount;
    }

    protected override void OnTargetHit(GameObject target)
    {
        if (m_InDeathAnim) return;
        base.OnTargetHit(target);
        if (target == m_CurrentTarget)
        {
            if (m_BounceCount > m_MaxBounces || Utils.GetAllEnemiesInRange(transform.position, m_AbilitySource.GetTotalStats().AOE * kBounceRadius).Count <= 1)
            {
                DestroySelf();
            }
            else
            {
                TargetNewEnemy(kBounceRadius);
                m_BounceCount++;
            }
        }
    }

    private void Update()
    {
        if (!m_CurrentTarget)
        {
            TargetNewEnemy(kBounceRadius);
            return;
        }

        Vector2 dir = Utils.GetDirectionToGameObject(transform.position, m_CurrentTarget);

        GetComponent<Rigidbody2D>().velocity = dir * m_AbilitySource.GetTotalStats().speed;

        // Rotate projectile in direction of travel
        Utils.PointInDirection(dir, gameObject);
    }

    private void TargetNewEnemy(float radius)
    {
        List<GameObject> enemies = Utils.GetAllEnemiesInRange(transform.position, m_AbilitySource.GetTotalStats().AOE * radius);

        if (enemies.Count <= 1)
        {
            Destroy(gameObject);
        }
        else
        {
            int loopCount = 0;
            GameObject newTarget;
            do
            {
                newTarget = enemies[Random.Range(0, enemies.Count)];
                loopCount++;
                if (loopCount > 100)
                {
                    Destroy(gameObject);
                    return;
                }
            } while (m_CurrentTarget == newTarget);
            m_CurrentTarget = newTarget;
            m_BounceCount++;
        }
    }

    protected override void DestroySelf()
    {
        GetComponent<Animator>().Play("Explode");
        m_InDeathAnim = true;
        StartCoroutine(Utils.DelayedCall(0.2f, () =>
        {
            base.DestroySelf();
        }));
    }
}