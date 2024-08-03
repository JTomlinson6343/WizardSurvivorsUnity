using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDebuffAOE : ConstantDamageAOE
{
    protected Debuff m_DebuffData;

    protected override void DamageTarget(GameObject enemy)
    {
        DebuffManager.m_Instance.AddDebuff(enemy.GetComponent<Actor>(), m_DebuffData);
    }
}
