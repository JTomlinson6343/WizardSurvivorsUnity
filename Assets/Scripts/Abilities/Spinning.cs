using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : Ability
{
    public override void OnChosen()
    {
        ProjectileManager.m_Instance.ShootMultipleSpinning(200, Color.blue, 0.7f, 3, 3);
    }
}
