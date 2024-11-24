using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    private bool m_ArmorBroken = false;

    [SerializeField] float m_UnamoredSpeedMult;
    [SerializeField] float m_UnamoredDamageMult;

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    public override void Update()
    {
        if (m_IsMidAnimation) return;
        base.Update();
    }

    public override DamageOutput OnDamage(float amount)
    {
        if (!m_ArmorBroken && m_Health <= m_MaxHealth / 2f)
        {
            ArmorBreak();
        }
        return base.OnDamage(amount);
    }

    private void ArmorBreak()
    {
        m_ArmorBroken = true;

        m_ContactDamage *= m_UnamoredDamageMult;
        m_Speed *= m_UnamoredSpeedMult;
        m_DamageResistance -= 0.25f;

        m_Animator.Play("ArmorBreak");
        m_AnimatorMask.Play("ArmorBreak");

        AudioManager.m_Instance.PlaySound(32);
    }
}
