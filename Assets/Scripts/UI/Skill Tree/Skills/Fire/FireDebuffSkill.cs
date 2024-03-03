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
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Fire) return;
        if (damageInstance.isDoT) return;

        ApplyFireDebuff(damageInstance.user,damageInstance.target);
    }

    public void ApplyFireDebuff(GameObject user, GameObject target)
    {
        Debuff debuff;
        switch (m_Data.level)
        {
            case 1:
                debuff = new Debuff(DebuffType.Blaze, m_DamageType, m_Damage, 1, 5, user);
                break;
            case 2:
                debuff = new Debuff(DebuffType.Blaze, m_DamageType, m_Damage, 3, 5, user);
                break;
            default:
                return;
        }

        DebuffManager.m_Instance.AddDebuff(target.GetComponent<Actor>(), debuff);
    }
}