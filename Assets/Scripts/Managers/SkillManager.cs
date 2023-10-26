using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager m_Instance;

    static List<SkillData> m_Skills = new();

    private void Awake()
    {
        m_Instance = this;
        ActivateSkills();
    }

    public void ResetSkillsAdded()
    {
        m_Skills.Clear();
    }
    public void AddSkill(SkillData skillID)
    {
        m_Skills.Add(skillID);
    }

    private void ActivateSkills()
    {
        Skill[] skills = GetComponentsInChildren<Skill>();
        if (skills == null)
            return;

        foreach(Skill skill in skills)
        {
            foreach(SkillData skillData in m_Skills)
            {
                if (skill.m_Data.id == skillData.id)
                {
                    skill.Init(skillData);
                }
                else
                {
                    skill.gameObject.SetActive(false);
                }
            }
        }
    }
}