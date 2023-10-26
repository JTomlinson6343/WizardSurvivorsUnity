using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject m_DamageNumberPrefab;

    [HideInInspector] public Ability m_AbilitySource;

    public void StartLifetimeTimer(float lifetime)
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnEnemyHit(GameObject enemy)
    {
        DamageEnemy(enemy);
        DestroySelf();
    }

    protected void DamageEnemy(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData();
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Info.damageType;
        data.userType = ActorType.Player;
        DamageManager.m_Instance.DamageInstance(data, enemy, transform.position, true, true);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnEnemyHit(collision.gameObject);
        }
    }
}
