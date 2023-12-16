using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBuffSkill : Skill
{
    public AbilityStats m_StatBuffs;
    public DamageType m_DamageType;

    public override void Init(SkillData data)
    {
        base.Init(data);

        for (int i = 0; i < data.level; i++)
        {
            AbilityManager.m_Instance.AddElementalAbilityBonusStats(m_DamageType, m_StatBuffs);
        }
    }
}