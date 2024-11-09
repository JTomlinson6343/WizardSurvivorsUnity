using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElementalStatBuffItem : Item
{
    [System.Serializable]
    struct ElementalAbilityStats
    {
        public DamageType damageType;
        public AbilityStats stat;
    }
    [SerializeField] ElementalAbilityStats[] m_ElementalAbilityStatBuffs;

    public override void LevelUp()
    {
        foreach (ElementalAbilityStats eStats in m_ElementalAbilityStatBuffs)
        {
            AbilityManager.m_Instance.AddElementalAbilityBonusStats(eStats.damageType, eStats.stat);
        }

        base.LevelUp();
    }
}