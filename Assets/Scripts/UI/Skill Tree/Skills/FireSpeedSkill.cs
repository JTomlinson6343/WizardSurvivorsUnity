using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FireSpeedSkill : CooldownSkill
{
    [SerializeField] PlayerStats m_BonusSpeed;

    [SerializeField] float m_Duration;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Fire) return;

        StartCooldown();

        Debug.Log("Player's speed increased by" + m_BonusSpeed.speed.ToString());

        Player.m_Instance.AddTempStats(m_BonusSpeed, m_Duration);
    }
}