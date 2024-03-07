using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class IceFreezeChance : Skill
{
    [SerializeField] float m_BaseChance;
    [SerializeField] float m_UpgradedChance;

    [SerializeField] float m_Duration;
    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;
        if (DebuffManager.GetDebuffIfPresent(damageInstance.target.GetComponent<Actor>(), Debuff.DebuffType.FrozenSkill) != null) return;
        if (damageInstance.damageType != DamageType.Frost) return;

        TryFreeze(damageInstance.user, damageInstance.target);
    }

    public void TryFreeze(GameObject user, GameObject target)
    {
        float chance = 0f;

        switch (m_Data.level)
        {
            case 1:
                chance = m_BaseChance;
                break;
            case 2:
                chance = m_UpgradedChance;
                break;
            default:
                break;
        }

        if (Random.Range(0f, 1f) > chance) return;
        
        Debuff debuff = new Debuff(Debuff.DebuffType.FrozenSkill, DamageType.None, 0f, 1, m_Duration, user);

        DebuffManager.m_Instance.AddDebuff(target.GetComponent<Actor>(), debuff);

        AudioManager.m_Instance.PlaySound(20);
    }
}