using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDebuffAOE : ConstantDamageAOE
{
    private Debuff m_DebuffData;

    private void Start()
    {
        m_DebuffData = new Debuff(Debuff.DebuffType.Curse, DamageType.None, 0, 1, 0.5f, gameObject);
    }
    protected override void DamageTarget(GameObject enemy)
    {
        DebuffManager.m_Instance.AddDebuff(enemy.GetComponent<Actor>(), m_DebuffData);
    }
}
