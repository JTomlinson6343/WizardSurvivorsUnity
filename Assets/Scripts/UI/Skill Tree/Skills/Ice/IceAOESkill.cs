using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAOESkill : CooldownSkill
{
    [SerializeField] GameObject m_AOEObject;
    [SerializeField] float m_Lifetime;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
        if (m_Data.level >= 2)
            m_Cooldown *= 0.8f;

        GetComponent<Ability>().UpdateTotalStats();
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Frost) return;
        if (!damageInstance.didKill) return;

        StartCooldown();

        GameObject aoe = Instantiate(m_AOEObject);
        ConstantDamageAOE constantDamageAOE = aoe.GetComponent<ConstantDamageAOE>();

        constantDamageAOE.Init(damageInstance.target.transform.position, GetComponent<Ability>(), m_Lifetime);
    }
}
