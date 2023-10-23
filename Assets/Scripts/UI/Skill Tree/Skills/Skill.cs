using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour
{
    // List of skills that unlock this skill
    [SerializeField] protected Skill[] m_Prerequisites;

    public bool m_Unlocked;

    public string m_SkillName;

    public string m_Description;

    public int m_Cost;

    public int m_SkillLevel;

    public int m_MaxSkillLevel;

    [SerializeField] protected Image m_Icon;

    private SkillTree m_SkillTreeRef;
    private Button m_ButtonRef;

    private void Start()
    {
        m_SkillTreeRef = GetComponentInParent<SkillTree>();
        m_ButtonRef = GetComponent<Button>();
        m_ButtonRef.onClick.AddListener(OnClick);
    }

    public bool IsMaxed()
    {
        return m_SkillLevel >= m_MaxSkillLevel;
    }

    public bool CheckPrerequisites()
    {
        if (m_Prerequisites.Length == 0)
        {
            return true;
        }

        bool canEnable = false;
        foreach (Skill preReq in m_Prerequisites)
        {
            canEnable = canEnable || preReq.m_Unlocked;
        }
        return canEnable;
    }

    private void OnClick()
    {
        m_SkillTreeRef.SetHighlightedSkill(this);
    }
}