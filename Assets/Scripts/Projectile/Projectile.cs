using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ability m_AbilitySource;
    protected int m_PierceCount;

    protected List<GameObject> m_HitTargets = new List<GameObject>();

    virtual public void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        transform.position = pos;

        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;

        // Rotate projectile in direction of travel
        GameplayManager.PointInDirection(dir, gameObject);

        m_PierceCount = m_AbilitySource.GetTotalStats().pierceAmount;
    }

    public void StartLifetimeTimer(float lifetime)
    {
        if (lifetime < 0f) return;

        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnTargetHit(GameObject target)
    {
        if (m_HitTargets.Contains(target)) return;

        m_HitTargets.Add(target);

        StartCoroutine(EndTargetCooldown(target));

        target.GetComponent<Actor>().KnockbackRoutine(GetComponent<Rigidbody2D>().velocity, m_AbilitySource.GetTotalStats().knockback);
        DamageTarget(target);

        if (m_AbilitySource.GetTotalStats().infinitePierce) return;
        if (m_AbilitySource.GetTotalStats().neverPierce) DestroySelf();

        if (m_PierceCount > 0)
            m_PierceCount--;
        else
        {
            DestroySelf();
        }
    }

    protected IEnumerator EndTargetCooldown(GameObject enemy)
    {
        yield return new WaitForSeconds(0.1f);

        m_HitTargets.Remove(enemy);
    }

    virtual protected void DamageTarget(GameObject target)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, target);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Data.damageType;
        data.target = target;
        data.abilitySource = m_AbilitySource;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }

    virtual protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    virtual public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.isTrigger)
        {
            OnTargetHit(collision.gameObject);
        }
    }
}
