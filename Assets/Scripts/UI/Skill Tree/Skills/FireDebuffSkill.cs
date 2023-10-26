using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuffSkill : Skill
{
    public override void Init()
    {
        base.Init();
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (damageInstance.damageType != DamageType.Fire) return;


    }
}