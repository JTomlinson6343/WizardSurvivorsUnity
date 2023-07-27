using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    private static Skill m_CurrentSkill;

    public void SetHighlightedSkill(Skill skill)
    {
        m_CurrentSkill = skill;
    }

    public void EnableHighlightedSkill()
    {

    }
}
