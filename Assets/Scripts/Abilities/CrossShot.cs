using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Ability
{
    public override void OnCast()
    {
        ProjectileManager.m_Instance.MultiShot(Player.m_Instance.GetStaffTransform().position, m_TotalStats.speed, 4, 1.0f, 0.5f);
    }
}
