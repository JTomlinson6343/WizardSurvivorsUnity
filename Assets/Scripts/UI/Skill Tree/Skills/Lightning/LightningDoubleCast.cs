using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDoubleCast : Skill
{
    public override void Init(SkillData data)
    {
        base.Init(data);

        lightningDoubleCastOn = true;
    }
}
