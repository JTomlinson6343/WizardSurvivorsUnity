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
        DamageManager.m_Instance.DamageInstance(enemy, m_AbilitySource.GetTotalStats().damage, transform.position, true, true);
        DestroySelf();
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
