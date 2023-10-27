using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireDebuffSkill : Skill
{
    public GameObject m_FireParticlePrefab;
    public Light2D m_LightPrefab;


    public float m_Damage;
    public DamageType m_DamageType;
    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (damageInstance.user.GetComponent<Actor>().m_ActorType != ActorType.Player) return;
        if (damageInstance.damageType != DamageType.Fire) return;
        if (damageInstance.isDoT) return;

        FireDebuff debuff = damageInstance.target.AddComponent<FireDebuff>();
        debuff.m_FireParticlePrefab = m_FireParticlePrefab;
        debuff.m_LightPrefab = m_LightPrefab;
        switch (m_Data.level)
        {
            case 1:
                debuff.Init(5, m_Damage, m_DamageType, damageInstance.user, true, 1);
                break;
            case 2:
                debuff.Init(5, m_Damage, m_DamageType, damageInstance.user, true, 3);
                break;
            default:
                break;
        }
    }
}