using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LightningStunCDR : CooldownSkill
{
    [SerializeField] AbilityStats m_BonusStats;

    [SerializeField] float m_Duration;
    [SerializeField] float m_DurationUpgradeModifier;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (!damageInstance.target.GetComponent<Debuff>()) return;
        if (!(damageInstance.target.GetComponent<Debuff>().m_DebuffType == DebuffType.Paralysed)) return;
        if (!damageInstance.didKill) return;

        if (m_Data.level == 2) m_Duration *= m_DurationUpgradeModifier;

        StartCooldown();

        AbilityManager.m_Instance.AddTempAbilityStatBuffs(m_BonusStats, m_Duration);
        m_Active = true;
        Invoke(nameof(ResetActiveFlag), m_Duration);

        AudioManager.m_Instance.PlaySound(9);
    }
}