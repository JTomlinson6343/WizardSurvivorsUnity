using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NecroDrainSoulSkill : CooldownSkill
{
    [SerializeField] float m_HealAmount;
    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (!damageInstance.didKill) return;

        Player.m_Instance.Heal(m_HealAmount * m_Data.level);

        StartCooldown();
    }
}
