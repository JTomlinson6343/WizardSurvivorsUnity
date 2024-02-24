using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBuffSkill : Skill
{
    public AbilityStats m_StatBuffs;
    public Ability m_Ability;

    public override void Init(SkillData data)
    {
        base.Init(data);

        for (int i = 0; i < data.level; i++)
        {
            if (!m_Ability)
            {
                AbilityManager.m_Instance.AddAbilityStatBuffs(m_StatBuffs);
                continue;
            }

            m_Ability.AddBonusStats(m_StatBuffs);
        }

    }
}
