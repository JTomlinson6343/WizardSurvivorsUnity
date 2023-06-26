using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject m_DamageNumberPrefab;

    public float m_Damage;

    public void StartLifetimeTimer(float lifetime)
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    virtual protected void OnEnemyHit(GameObject enemy)
    {
        Actor actorComponent = enemy.GetComponent<Actor>();
        // Damage actor
        if (actorComponent.TakeDamage(m_Damage) == true)
        {
            // Spawn damage numbers
            GameObject damageNumber = Instantiate(m_DamageNumberPrefab);
            damageNumber.transform.position = this.transform.position;
            damageNumber.GetComponent<FloatingDamage>().m_Colour = Color.white;
            damageNumber.GetComponent<FloatingDamage>().m_Damage = m_Damage;

            DestroySelf();
        }

    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnEnemyHit(collision.gameObject);
        }
    }
}
