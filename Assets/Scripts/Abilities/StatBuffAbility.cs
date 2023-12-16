using System.Collections;
using UnityEngine;

public class StatBuffAbility : Ability
{
    [SerializeField] AbilityStats m_AbilityStatBuffs;
    [SerializeField] PlayerStats m_PlayerStatBuffs;

    public override void LevelUp()
    {
        AbilityManager.m_Instance.AddAbilityStatBuffs(m_AbilityStatBuffs);
        Player.m_Instance.AddBonusStats(m_PlayerStatBuffs);
        base.LevelUp();
    }
}