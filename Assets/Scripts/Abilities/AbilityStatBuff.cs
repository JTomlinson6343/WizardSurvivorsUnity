using System.Collections;
using UnityEngine;

public class AbilityStatBuff : Ability
{
    [SerializeField] AbilityStats m_StatBuffs;

    public override void OnChosen()
    {
        AbilityManager.m_Instance.AddAbilityStatBuffs(m_StatBuffs);
        base.OnChosen();
    }

    public override void LevelUp()
    {
        AbilityManager.m_Instance.AddAbilityStatBuffs(m_StatBuffs);
        base.LevelUp();
    }
}