using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Ability
{
    public float m_DamageScaling;
    public override void OnCast()
    {
        ProjectileManager.m_Instance.SpawnBlizzard(m_DamageScaling);
    }
}
