using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPPickup : Pickup
{
    static readonly float kSoundEffectCooldown = 0.05f;
    static float m_LastSoundTime;

    override protected void OnPickup()
    {
        ProgressionManager.m_Instance.AddXP(1);
        if (Time.time - m_LastSoundTime > kSoundEffectCooldown)
        {
            AudioManager.m_Instance.PlaySound(1, 0.5f);
            m_LastSoundTime = Time.time;
        }
        base.OnPickup();
    }
}
