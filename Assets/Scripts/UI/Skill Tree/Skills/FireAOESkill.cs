using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAOE : CooldownSkill
{
    [SerializeField] GameObject m_AOEObject;
    [SerializeField] float m_Lifetime;
    [SerializeField] float m_Scale;
    [SerializeField] float m_Damage;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
        if (m_Data.level >= 2)
            m_Cooldown *= 0.8f;
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Fire) return;
        if (!damageInstance.didKill) return;

        StartCooldown();

        GameObject aoe = Instantiate(m_AOEObject);
        FireAOESkillObject fireSkillAoe = aoe.GetComponent<FireAOESkillObject>();
        fireSkillAoe.Init(damageInstance.target.transform.position, m_Damage, m_Scale, m_Lifetime);
        
        AudioManager.m_Instance.PlaySound(2);
    }
}
