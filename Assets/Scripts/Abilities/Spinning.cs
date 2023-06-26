using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : Ability
{
    public override void OnChosen()
    {
        ProjectileManager.m_Instance.ShootMultipleSpinning(m_TotalStats.speed, m_TotalStats.damageScaling, m_TotalStats.AOE, m_TotalStats.amount);
    }
}
