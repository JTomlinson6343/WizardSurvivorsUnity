using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : Ability
{
    public override void OnCast()
    {
        ProjectileManager.m_Instance.ShootMultipleSpinning(200, Color.blue, 0.7f, 3, 1);
    }
}
