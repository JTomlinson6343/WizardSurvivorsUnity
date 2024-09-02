using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldGemSkill : Skill
{
    [SerializeField] float m_Chance;
    public override void Init(SkillData data)
    {
        base.Init(data);
        ProgressionManager.m_Instance.m_GoldGemChance = m_Chance * data.level;
    }
}
