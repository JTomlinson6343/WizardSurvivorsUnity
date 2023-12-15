using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPPickup : Pickup
{
    public float m_XPValue;
    override protected void OnPickup()
    {
        ProgressionManager.m_Instance.AddXP(m_XPValue);
        AudioManager.m_Instance.PlaySound(1);
        base.OnPickup();
    }
}
