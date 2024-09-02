using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollSkill : Skill
{
    [SerializeField] float m_Chance;
    public override void Init(SkillData data)
    {
        base.Init(data);
        AbilityManager.m_RerollChance = m_Chance * data.level;
    }
}
