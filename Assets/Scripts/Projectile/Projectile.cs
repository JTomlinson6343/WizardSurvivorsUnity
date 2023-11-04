using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Ability m_AbilitySource;

    public void StartLifetimeTimer(float lifetime)
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnEnemyHit(GameObject enemy)
    {
        enemy.GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity.normalized * 8f;
        DamageEnemy(enemy);
        DestroySelf();
    }

    virtual protected void DamageEnemy(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject,enemy);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Info.damageType;
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
