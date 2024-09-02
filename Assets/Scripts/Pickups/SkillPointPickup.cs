using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointPickup : Pickup
{
    [SerializeField] float m_ValueModifier = 1f;
    override protected void OnPickup()
    {
        ProgressionManager.m_Instance.AddSkillPoints((int)m_ValueModifier);
        AudioManager.m_Instance.PlayRandomPitchSound(13, 0.5f);
        base.OnPickup();
        TutorialManager.DisplayTutorial("Skill Gems");
    }
}
