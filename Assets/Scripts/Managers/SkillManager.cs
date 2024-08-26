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
        SingletonCheck();
        if (SkillPanel.m_Instance)
            SkillPanel.m_Instance.Init();
    }

    private void Start()
    {
        SingletonCheck();
        ActivateSkills();
    }

    private void SingletonCheck()
    {
        if (m_Instance != null) Destroy(this);
        else m_Instance = this;
    }

    public void ResetSkillsAdded()
    {
        m_Skills.Clear();
    }
    public void AddSkill(SkillData skillID)
    {
        m_Skills.Add(skillID);
    }

    // Called when the game starts and decides which skills are enabled
    private void ActivateSkills()
    {
        if (GetSkills().Length == 0) return;

        foreach (Skill skill in GetSkills())
        {
            foreach(SkillData skillData in m_Skills)
            {
                if (skill.m_Data.id == skillData.id && PlayerManager.m_SkillTreeRef.GetSkillIconWithID(skill.m_Data.id) || PlayerManager.m_GlobalSkillTreeRef.GetSkillIconWithID(skill.m_Data.id))
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

    public Skill[] GetSkills()
    {
        Skill[] skills = GetComponentsInChildren<Skill>();

        if (skills == null) return null;

        else return skills;
    }
    public CooldownSkill[] GetCooldownSkills()
    {
        CooldownSkill[] skills = GetComponentsInChildren<CooldownSkill>();

        if (skills == null) return null;

        else return skills;
    }
}