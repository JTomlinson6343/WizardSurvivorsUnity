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

    //public void ApplyFireDebuff(GameObject user, GameObject target)
    //{
    //    FireDebuff debuff = target.AddComponent<FireDebuff>();
    //    debuff.m_FireParticlePrefab = m_FireParticlePrefab;
    //    debuff.m_LightPrefab = m_LightPrefab;
    //    switch (m_Data.level)
    //    {
    //        case 1:
    //            debuff.Init(5, m_Damage, m_DamageType, user, false, 1, DebuffType.Blaze);
    //            break;
    //        case 2:
    //            debuff.Init(5, m_Damage, m_DamageType, user, false, 3, DebuffType.Blaze);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void ApplyFireDebuff(GameObject user, GameObject target)
    {
        Debuff debuff = new Debuff(DebuffType.Blaze, m_DamageType, m_Damage, 1, 5, user);

        DebuffManager.m_Instance.AddDebuff(target.GetComponent<Actor>(), debuff);
    }
}