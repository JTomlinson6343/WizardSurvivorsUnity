using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSkill : Skill
{
    [SerializeField] Fireball m_FireballRef;
    public override void Init(SkillData data)
    {
        base.Init(data);

        Flamethrower flamethrower = AbilityManager.m_Instance.GetComponentInChildren<Flamethrower>();
        // Inherit bonus stats from fireball
        flamethrower.AddBonusStats(m_FireballRef.GetBonusStats());
        flamethrower.Enable();

        Player.m_Instance.m_ActiveAbility = flamethrower;
    }
}
