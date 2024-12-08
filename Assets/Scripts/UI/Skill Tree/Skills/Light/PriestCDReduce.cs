using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestCDReduce : Skill
{
    [SerializeField] float m_RemainingCooldownReducedPercent = 1f;
    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;
        if (!damageInstance.didCrit) return;

        Spell[] allSpells = AbilityManager.m_Instance.GetAllSpells(); 
        foreach (Spell spell in allSpells)
        {
            spell.m_CooldownRemaining -= m_RemainingCooldownReducedPercent * spell.GetTotalStats().cooldown;
        }
    }
}
