using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroBleed : Skill
{
    [SerializeField] float m_Resistance = 0.1f;

    public override void Init(SkillData data)
    {
        base.Init(data);

        necroBleedEnabled = true;

        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.target.CompareTag("Player")) return;
        if (damageInstance.didKill) return;
        if (damageInstance.isDoT) return;

        float duration = damageInstance.amount/2f * Debuff.kTickRate;

        NecroBleedDebuff debuffData = new NecroBleedDebuff(Debuff.DebuffType.Bleed, DamageType.Physical, 1, 1, duration, damageInstance.target, m_Resistance);
        DebuffManager.m_Instance.AddDebuff(damageInstance.target.GetComponent<Actor>(), debuffData);
    }
}
