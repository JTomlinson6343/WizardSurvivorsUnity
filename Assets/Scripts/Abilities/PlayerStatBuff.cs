using System.Collections;
using UnityEngine;

public class PlayerStatBuff : Ability
{
    [SerializeField] PlayerStats m_StatBuffs;

    public override void OnChosen()
    {
        base.OnChosen();
        Player.m_Instance.AddBonusStats(m_StatBuffs);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        Player.m_Instance.AddBonusStats(m_StatBuffs);
    }
}