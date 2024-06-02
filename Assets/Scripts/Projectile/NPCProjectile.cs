using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCProjectile : Projectile
{
    public bool m_InfinitePierce;
    protected float m_Damage;
    protected float m_Knockback;
    protected GameObject m_User;
    protected DamageType m_DamageType;
    public void Init(Vector2 pos, Vector2 dir, float speed, float lifetime, float damage, float knockback, GameObject user, DamageType damageType)
    {
        m_Damage = damage;
        m_Knockback = knockback;
        m_User = user;
        m_DamageType = damageType;

        StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        transform.position = pos;

        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;

        // Rotate projectile in direction of travel
        Utils.PointInDirection(dir, gameObject);
    }

    protected override void OnTargetHit(GameObject target)
    {
        target.GetComponent<Actor>().KnockbackRoutine(GetComponent<Rigidbody2D>().velocity, m_Knockback);
        DamageTarget(target);

        if (m_InfinitePierce) return;

        DestroySelf();
    }

    protected override void DamageTarget(GameObject target)
    {
        DamageInstanceData data = new DamageInstanceData(m_User, target);
        data.amount = m_Damage;
        data.damageType = m_DamageType;
        data.target = target;
        data.doDamageNumbers = true;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Replace with what you want to target

        //if (collision.gameObject.CompareTag("Player") && collision.isTrigger)
        //{
        //    OnTargetHit(collision.gameObject);
        //}
    }
}
