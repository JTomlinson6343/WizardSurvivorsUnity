using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : Spell
{
    [SerializeField] float m_Radius;
    public override void OnCast()
    {
        base.OnCast();

        ProjectileManager.m_Instance.ShootMultipleSpinning(m_TotalStats.speed, this, m_Radius, m_TotalStats.castAmount);

        CastSound();
    }
}
