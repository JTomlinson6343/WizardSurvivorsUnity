using System.Collections;
using UnityEngine;

public class AbilityStatBuff : Ability
{
    [SerializeField] AbilityStats m_StatBuffs;

    public override void OnChosen()
    {
        base.OnChosen();
        AbilityManager.m_Instance.AddAbilityStatBuffs(m_StatBuffs);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        AbilityManager.m_Instance.AddAbilityStatBuffs(m_StatBuffs);
    }
}