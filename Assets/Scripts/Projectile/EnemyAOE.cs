using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class EnemyAOE : EnemyProjectile
{
    public virtual void Init(Vector2 pos, float damage, float knockback, float lifetime, GameObject user, DamageType damageType)
    {
        m_Damage = damage;
        m_Knockback = knockback;
        m_User = user;
        m_DamageType = damageType;

        StartLifetimeTimer(lifetime);
        transform.position = pos;
    }
    protected override void OnTargetHit(GameObject enemy)
    {
        if (enemy.GetComponent<Player>().m_IsInvincible) return;

        enemy.GetComponent<Actor>().KnockbackRoutine(GetComponent<Rigidbody2D>().velocity, m_Knockback);
        DamageTarget(enemy);
    }

    protected override void DamageTarget(GameObject target)
    {
        DamageInstanceData data = new DamageInstanceData(m_User, target);
        data.amount = m_Damage;
        data.damageType = m_DamageType;
        data.target = target;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }
}
