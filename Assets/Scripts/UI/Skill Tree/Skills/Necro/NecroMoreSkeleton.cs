using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMoreSkeleton : Skill
{
    public override void Init(SkillData data)
    {
        base.Init(data);

        RaiseDead.m_Instance.m_SkeletonsSpawned += data.level;
    }
}
