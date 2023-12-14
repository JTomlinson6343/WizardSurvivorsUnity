using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEObject : Projectile
{
    protected override void OnEnemyHit(GameObject enemy)
    {
        DamageEnemy(enemy);
    }

    public void Init(Vector2 pos, Ability ability, float lifetime)
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