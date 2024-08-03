using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDebuffAOE : ConstantDamageAOE
{
    protected Debuff m_DebuffData;

    public override void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        base.Init(pos, ability, lifetime);
    }

    protected override void DamageTarget(GameObject enemy)
    {
        DebuffManager.m_Instance.AddDebuff(enemy.GetComponent<Actor>(), m_DebuffData);
    }
}
