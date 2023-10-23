using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager m_Instance;

    List<Skill> m_Skills;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        m_Skills = new List<Skill>();
    }

    public void AddSkill(Skill skill)
    {
        m_Skills.Add(skill);
    }
}
