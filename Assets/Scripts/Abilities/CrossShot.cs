using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Ability
{
    public override void OnCast()
    {
        ProjectileManager.m_Instance.MultiShot(m_PlayerRef.transform.position, 10, Color.red, 4, 1.0f, 0.5f);
    }
}
