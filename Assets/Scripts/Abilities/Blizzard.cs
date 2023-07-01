using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Ability
{
    GameObject m_AOEObject;
    public override void OnCast()
    {
        m_AOEObject = ProjectileManager.m_Instance.SpawnBlizzard(m_TotalStats.damageScaling, m_TotalStats.AOE);
    }
}
