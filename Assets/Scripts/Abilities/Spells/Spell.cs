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
}
