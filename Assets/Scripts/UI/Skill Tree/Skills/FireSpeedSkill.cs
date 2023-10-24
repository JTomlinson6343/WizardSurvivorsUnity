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
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    override public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (damageInstance.damageType != DamageType.Fire) return;

        Debug.Log("Player's speed increased by" + bonusSpeed.speed.ToString());

        Player.m_Instance.AddTempStats(bonusSpeed, 2.0f);
    }
}