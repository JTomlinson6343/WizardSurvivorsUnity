using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillIcon : MonoBehaviour
{
    public bool m_Unlocked;

    public int m_SkillLevel;

    public int m_MaxSkillLevel;

    public string m_SkillName;

    public string m_Description;

    public int m_Cost;

    public SkillID m_SkillID;

    // List of skills that unlock this skill
    [SerializeField] protected SkillIcon[] m_Prerequisites;

    [SerializeField] protected Image m_Icon;

    private SkillTree m_SkillTreeRef;
    private Button m_ButtonRef;

    private void Start()
    {
        m_SkillTreeRef = GetComponentInParent<SkillTree>();
        m_ButtonRef = GetComponent<Button>();
        if (m_ButtonRef != null)
        {
            m_ButtonRef.onClick.AddListener(OnClick);
        }
    }

    public bool CheckPrerequisites()
    {
        if (m_Prerequisites.Length == 0)
        {
            return true;
        }

        bool canEnable = false;
        foreach (SkillIcon preReq in m_Prerequisites)
        {
            canEnable = canEnable || preReq.m_Unlocked;
        }
        return canEnable;
    }

    public bool IsMaxed()
    {
        return m_SkillLevel >= m_MaxSkillLevel;
    }

    private void OnClick()
    {
        m_SkillTreeRef.SetHighlightedSkill(this);
    }
}
