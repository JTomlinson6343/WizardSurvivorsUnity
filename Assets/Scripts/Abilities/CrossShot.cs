using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Ability
{
    public override void OnCast()
    {
        ProjectileManager.m_Instance.MultiShot(new Vector2(0, 0), 10, Color.red, 4, 10, 0.5f);
    }
}
