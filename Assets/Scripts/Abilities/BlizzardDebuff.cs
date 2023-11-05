using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class BlizzardDebuff : Debuff
{
    override protected void OnTick()
    {
        DamageInstanceData data = new DamageInstanceData(m_Source, gameObject);
        data.amount = m_Damage * m_StackAmount;
        data.damageType = m_DamageType;
        data.isDoT = true;
        data.doDamageNumbers = true;
        data.doIFrames = false;
        data.abilitySource = m_AbilitySource;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }

    protected override void EndDebuff()
    {
        base.EndDebuff();
    }
}
