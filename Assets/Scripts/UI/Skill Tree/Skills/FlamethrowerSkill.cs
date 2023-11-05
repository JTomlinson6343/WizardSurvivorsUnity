using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSkill : Skill
{
    public override void Init(SkillData data)
    {
        base.Init(data);

        Flamethrower flamethrower = AbilityManager.m_Instance.GetComponentInChildren<Flamethrower>();
        flamethrower.Enable();

        Player.m_Instance.m_ActiveAbility = flamethrower;
    }
}
