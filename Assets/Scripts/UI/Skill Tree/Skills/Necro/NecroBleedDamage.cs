using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroBleedDamage : Skill
{
    [SerializeField] AbilityStats m_BonusStats;

    public static AbilityStats stats;

    public override void Init(SkillData data)
    {
        base.Init(data);

        for (int i = 0; i < data.level; i++)
        {
            stats += m_BonusStats;
        }

        bleedDamageSkillActivated = true;
    }
}
