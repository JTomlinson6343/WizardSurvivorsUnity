using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEObject : Projectile
{
    protected override void OnEnemyHit(GameObject enemy)
    {
        if (m_HitEnemies.Contains(enemy)) return;

        m_HitEnemies.Add(enemy);

        StartCoroutine(EndEnemyCooldown(enemy));

        DamageEnemy(enemy);
    }

    protected override void DamageEnemy(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, enemy);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Data.damageType;
        data.target = enemy;
        data.abilitySource = m_AbilitySource;
        DamageManager.m_Instance.DamageInstance(data, enemy.transform.position);
    }

    public virtual void Init(Vector2 pos, Ability ability, float lifetime)
    {
        GetComponent<Projectile>().m_AbilitySource = ability;
        transform.localScale = new Vector2(ability.GetTotalStats().AOE, ability.GetTotalStats().AOE);
        GetComponent<Projectile>().StartLifetimeTimer(lifetime);
        transform.position = pos;
    }

    public void Init(GameObject parent, Vector2 pos, Ability ability, float lifetime)
    {
        transform.SetParent(parent.transform);
        Init(pos, ability, lifetime);
    }
}