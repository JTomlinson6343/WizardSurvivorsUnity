using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : Ability
{
    [SerializeField] Color m_Colour;

    public override void OnChosen()
    {
        ProjectileManager.m_Instance.ShootMultipleSpinning(m_TotalStats.speed, m_Colour, m_TotalStats.damageScaling, m_TotalStats.AOE, m_TotalStats.amount);
    }
}
