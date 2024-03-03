using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : Skill
{
    [SerializeField] PlayerStats m_StatBuffs;

    [SerializeField] float m_ResistanceBuffs;

    public override void Init(SkillData data)
    {
        base.Init(data);
        for (int i = 0; i < data.level; i++)
        {
            Player.m_Instance.AddBonusStats(m_StatBuffs);
            Player.m_Instance.m_DamageResistance += m_ResistanceBuffs;
        }
    }
}
