using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    protected override void OnPickup()
    {
        Player.m_Instance.PercentHeal(0.2f);

        base.OnPickup();
    }
}
