using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell : Ability
{
    public enum SpellTag
    {
        Offensive,
        Defensive,
        Summon,
        Stun,
        AOE,
        Piercing,
        DOT,
        Debuff,
        Buff
    }

    [SerializeField] SpellTag[] m_SpellTags;

    public bool HasTag(SpellTag tag)
    {
        if (m_SpellTags == null) return false;

        if (m_SpellTags.Contains(tag)) return true;

        return false;
    }

    public override void OnChosen()
    {
        base.OnChosen();
        m_isMaxed = true;
    }
    public override void UpdateTotalStats()
    {
        if (!AbilityManager.m_Instance) return;

        base.UpdateTotalStats();

        if (HasTag(SpellTag.Summon))
        {
            m_TotalStats.damage += m_BonusStats.summonDamage * m_BaseStats.damage + AbilityManager.m_Instance.GetAbilityStatBuffs().summonDamage * m_BaseStats.damage;
        }
    }
}
