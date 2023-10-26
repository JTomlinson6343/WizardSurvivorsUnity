using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuffSkill : Skill
{
    public float m_Damage;
    public DamageType m_DamageType;
    public override void Init()
    {
        base.Init();
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (damageInstance.user.GetComponent<Actor>().m_ActorType != ActorType.Player) return;
        if (damageInstance.damageType != DamageType.Fire) return;
        if (damageInstance.isDoT) return;

        damageInstance.target.AddComponent<Debuff>().Init(5, m_Damage, m_DamageType, damageInstance.user, true, 1);
    }
}