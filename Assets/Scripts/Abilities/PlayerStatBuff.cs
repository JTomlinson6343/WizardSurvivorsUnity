using System.Collections;
using UnityEngine;

public class PlayerStatBuff : Ability
{
    [SerializeField] PlayerStats m_StatBuffs;

    public override void OnChosen()
    {
        Player.m_Instance.AddBonusStats(m_StatBuffs);
        base.OnChosen();
    }

    public override void LevelUp()
    {
        Player.m_Instance.AddBonusStats(m_StatBuffs);
        base.LevelUp();
    }
}