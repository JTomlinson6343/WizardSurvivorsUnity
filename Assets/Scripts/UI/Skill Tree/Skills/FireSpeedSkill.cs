using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class FireSpeedSkill : Skill
{
    [SerializeField] PlayerStats bonusSpeed;

    public override void Init()
    {
        base.Init();
        Actor.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    private void OnDamageInstance(DamageInstance damageInstance)
    {
        if (damageInstance.damageType != DamageType.Fire) return;

        Player.m_Instance.AddTempStats(bonusSpeed, 2);
    }
}