using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float lifetime;

    public void StartLifetimeTimer()
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    private void DestroySelf()
    {
        Destroy(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject enemy = collision.gameObject;
            //enemy.GetComponent<EnemyLogic>().TakeDamage(damage);
            DestroySelf();
        }
    }
}
