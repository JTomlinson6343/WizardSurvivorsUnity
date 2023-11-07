using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSkill : Skill
{
    public Fireball m_FireballRef;
    public override void Init(SkillData data)
    {
        base.Init(data);

        Flamethrower flamethrower = AbilityManager.m_Instance.GetComponentInChildren<Flamethrower>();
        flamethrower.Enable();
        // Inherit bonus stats from fireball
        if (m_FireballRef != null)
        {
            flamethrower.AddBonusStats(m_FireballRef.GetBonusStats());
            flamethrower.UpdateTotalStats();
        }

        Player.m_Instance.m_ActiveAbility = flamethrower;
    }
}
