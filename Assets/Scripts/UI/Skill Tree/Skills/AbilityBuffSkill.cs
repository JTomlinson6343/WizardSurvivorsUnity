using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBuffSkill : Skill
{
    [SerializeField] AbilityStats m_StatBuffs;

    public override void Init(SkillData data)
    {
        base.Init(data);

        AbilityManager.m_Instance.AddAbilityStatBuffs(m_StatBuffs);
    }
}
