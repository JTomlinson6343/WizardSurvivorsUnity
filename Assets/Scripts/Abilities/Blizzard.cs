using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Ability
{
    public override void OnCast()
    {
        ProjectileManager.m_Instance.SpawnBlizzard(m_TotalStats.damageScaling, m_TotalStats.AOE);
    }
}
