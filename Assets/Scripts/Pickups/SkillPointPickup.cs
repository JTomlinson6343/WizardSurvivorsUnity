using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointPickup : Pickup
{
    override protected void OnPickup()
    {
        ProgressionManager.m_Instance.AddSkillPoints(1);
        AudioManager.m_Instance.PlaySound(13, 0.5f);
        base.OnPickup();
    }
}
