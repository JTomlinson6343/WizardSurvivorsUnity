using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : Skill
{
    [SerializeField] PlayerStats m_StatBuffs;

    public override void Init(SkillData data)
    {
        base.Init(data);

        Player.m_Instance.AddBonusStats(m_StatBuffs);
    }
}
