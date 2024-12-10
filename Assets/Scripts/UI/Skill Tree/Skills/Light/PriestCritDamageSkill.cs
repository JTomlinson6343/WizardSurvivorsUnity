using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestCritDamageSkill : Skill
{
    [SerializeField] float m_IncreaseAmount = 0.25f; 

    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_Instance.m_CritDamageModifier *= 1f + m_IncreaseAmount * data.level;
    }
}
