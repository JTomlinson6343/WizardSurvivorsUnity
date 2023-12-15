using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class XPPickup : Pickup
{
    public int xpValue;
    override protected void OnPickup()
    {
        ProgressionManager.m_Instance.AddXP(xpValue);
    }
}
