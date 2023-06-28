using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject m_DamageNumberPrefab;

    [HideInInspector] public float m_DamageScaling;

    public void StartLifetimeTimer(float lifetime)
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnEnemyHit(GameObject enemy)
    {
        Player.m_Instance.DamageInstance(enemy, m_DamageScaling, transform.position);
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
