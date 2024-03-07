using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IceSlowSkill : Skill
{
    [SerializeField] float m_BaseSlow;
    [SerializeField] float m_UpgradedSlow;
    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Frost) return;

        ApplySlowDebuff(damageInstance.user, damageInstance.target);
    }

    public void ApplySlowDebuff(GameObject user, GameObject target)
    {
        SlowDebuff debuff;
        switch (m_Data.level)
        {
            case 1:
                debuff = new SlowDebuff(Debuff.DebuffType.Frostbite, DamageType.None, 0f, 1, 1f, user, m_BaseSlow);
                break;
            case 2:
                debuff = new SlowDebuff(Debuff.DebuffType.Frostbite, DamageType.None, 0f, 3, 1f, user, m_UpgradedSlow);
                break;
            default:
                return;
        }

        DebuffManager.m_Instance.AddDebuff(target.GetComponent<Actor>(), debuff);
    }
}