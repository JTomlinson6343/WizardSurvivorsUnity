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
        m_Ability.AddBonusStats(m_StatBuffs);
    }
}
