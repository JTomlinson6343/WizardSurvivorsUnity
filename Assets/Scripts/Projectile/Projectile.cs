using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Ability m_AbilitySource;

    protected List<GameObject> m_HitEnemies = new List<GameObject>();

    public void StartLifetimeTimer(float lifetime)
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnEnemyHit(GameObject enemy)
    {
        if (m_HitEnemies.Contains(enemy)) return;

        m_HitEnemies.Add(enemy);

        StartCoroutine(EndEnemyCooldown(enemy));

        enemy.GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity.normalized * m_AbilitySource.GetTotalStats().knockback;
        DamageEnemy(enemy);
        DestroySelf();
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

    private void DestroySelf()
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
