using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellBuffSkill : Skill
{
    public AbilityStats m_StatBuffs;

    public override void Init(SkillData data)
    {
        base.Init(data);

        for (int i = 0; i < data.level; i++)
        {
            if (Player.m_Instance.m_ActiveAbility)
            {
                Player.m_Instance.m_ActiveAbility.AddBonusStats(m_StatBuffs);
            }
        }
    }
}
