using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPPickup : Pickup
{
    override protected void OnPickup()
    {
        ProgressionManager.m_Instance.AddXP(1);
        AudioManager.m_Instance.PlaySound(1);
        base.OnPickup();
    }
}
