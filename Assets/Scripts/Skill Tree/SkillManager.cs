using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillIcon m_CurrentSkill;

    public void SetHighlightedSkill(SkillIcon skill)
    {
        m_CurrentSkill = skill;
    }
}
