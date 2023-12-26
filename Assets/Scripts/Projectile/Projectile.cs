using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
     public Ability m_AbilitySource;
    protected int m_PierceCount;

    protected List<GameObject> m_HitEnemies = new List<GameObject>();

    virtual public void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        transform.position = pos;

        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;

        // Rotate projectile in direction of travel
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

        m_PierceCount = m_AbilitySource.GetTotalStats().pierceAmount;
    }

    public void StartLifetimeTimer(float lifetime)
    {
        if (lifetime < 0f) return;

        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnEnemyHit(GameObject enemy)
    {
        if (m_HitEnemies.Contains(enemy)) return;

        m_HitEnemies.Add(enemy);

        StartCoroutine(EndEnemyCooldown(enemy));

        enemy.GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity.normalized * m_AbilitySource.GetTotalStats().knockback;
        DamageEnemy(enemy);

        if (m_AbilitySource.GetTotalStats().infinitePierce) return;
        if (m_AbilitySource.GetTotalStats().neverPierce) DestroySelf();

        if (m_PierceCount > 0)
            m_PierceCount--;
        else
        {
            DestroySelf();
        }
    }

    protected IEnumerator EndEnemyCooldown(GameObject enemy)
    {
        yield return new WaitForSeconds(0.1f);

        m_HitEnemies.Remove(enemy);
    }

    virtual protected void DamageEnemy(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject,enemy);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Data.damageType;
        data.target = enemy;
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
            OnEnemyHit(collision.gameObject);
        }
    }
}
