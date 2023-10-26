using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSkill : Skill
{
    public float m_Cooldown;
    protected bool m_OnCooldown = false;

    protected void StartCooldown()
    {
        m_OnCooldown = true;
        Invoke(nameof(EndCooldown), m_Cooldown);
    }

    private void EndCooldown()
    {
        m_OnCooldown = false;
    }
}
