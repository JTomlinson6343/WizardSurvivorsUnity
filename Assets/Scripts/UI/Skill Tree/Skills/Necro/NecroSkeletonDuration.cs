using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroSkeletonDuration : Skill
{
    [SerializeField] float m_DurationModifier = 0.2f;
    public override void Init(SkillData data)
    {
        base.Init(data);

        RaiseDead.m_Instance.m_DurationModifier *= 1f + m_DurationModifier * data.level;
    }
}
