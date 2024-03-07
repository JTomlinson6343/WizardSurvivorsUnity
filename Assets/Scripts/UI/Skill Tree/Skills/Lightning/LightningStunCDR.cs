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

    [SerializeField] GameObject m_Sparks;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (DebuffManager.GetDebuffIfPresent(damageInstance.target.GetComponent<Actor>(), Debuff.DebuffType.Paralysed) == null) return;
        if (!damageInstance.didKill) return;

        if (m_Data.level == 2) m_Duration *= m_DurationUpgradeModifier;

        StartCooldown();

        AbilityManager.m_Instance.AddTempAbilityStatBuffs(m_BonusStats, m_Duration);

        GameObject sparksVFX = Instantiate(m_Sparks);
        sparksVFX.transform.SetParent(Player.m_Instance.transform);
        sparksVFX.transform.position = Player.m_Instance.m_DebuffPlacement.transform.position;
        sparksVFX.GetComponent<DestroyAfterAnimation>().m_Lifetime = m_Duration;

        m_Active = true;
        Invoke(nameof(ResetActiveFlag), m_Duration);

        AudioManager.m_Instance.PlaySound(9);
    }
}