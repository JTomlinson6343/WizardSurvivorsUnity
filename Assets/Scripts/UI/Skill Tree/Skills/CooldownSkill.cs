using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CooldownSkill : Skill
{
    public float m_Cooldown;
    protected bool m_OnCooldown = false;

    public HUDSkillIcon m_HUDSkillIconRef;

    public override void Init(SkillData data)
    {
        base.Init(data);
    }

    public void InitHUDSkillIcon(HUDSkillIcon skillIcon)
    {
        m_HUDSkillIconRef = skillIcon;
        m_HUDSkillIconRef.Init(m_Data);
    }

    protected void StartCooldown()
    {
        m_OnCooldown = true;
        LeanTween.delayedCall(m_Cooldown, () =>
        {
            m_OnCooldown = false;
        });
        m_HUDSkillIconRef.StartCooldown(m_Cooldown);
    }
}
