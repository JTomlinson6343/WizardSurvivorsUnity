using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAOE : CooldownSkill
{
    [SerializeField] GameObject m_AOEObject;
    [SerializeField] float m_Lifetime;
    [SerializeField] float m_Damage; // percent max hp
    [SerializeField] FireDebuffSkill m_FireDebuffSkillRef;

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
        if (!damageInstance.didKill) return;

        GameObject aoe = Instantiate(m_AOEObject);
        FireAOESkillObject fireSkillAoe = GetComponent<FireAOESkillObject>();
        fireSkillAoe.StartLifetimeTimer(m_Lifetime);
        fireSkillAoe.m_Damage = m_Damage;
        fireSkillAoe.m_FireDebuffSkillRef = m_FireDebuffSkillRef;
    }
}
