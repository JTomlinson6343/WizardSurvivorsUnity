using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : Ability
{
    public override void OnCast()
    {
        base.OnCast();

        ProjectileManager.m_Instance.ShootMultipleSpinning(m_TotalStats.speed, this, m_TotalStats.AOE, m_TotalStats.amount);
    }
}
