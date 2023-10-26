using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager m_Instance;

    static List<SkillID> m_Skills = new();

    private void Awake()
    {
        m_Instance = this;
        ActivateSkills();
    }

    public void ResetSkillsAdded()
    {
        m_Skills.Clear();
    }
    public void AddSkill(SkillID skillID)
    {
        m_Skills.Add(skillID);
    }

    private void ActivateSkills()
    {
        Skill[] skills = GetComponentsInChildren<Skill>();
        if (skills == null)
            return;

        foreach (Skill skill in skills)
        {
            if (m_Skills.Contains(skill.m_ID))
            {
                skill.Init();
            }
            else
            {
                skill.gameObject.SetActive(false);
            }
        }
    }
}