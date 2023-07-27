using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour
{
    // List of skills that unlock this skill
    [SerializeField] protected Skill[] m_Prerequisites;

    public string m_SkillName;

    public string m_Description;

    [SerializeField] protected Image m_Icon;

    private SkillTree m_SkillTreeRef;
    private Button m_ButtonRef;

    private void Start()
    {
        m_SkillTreeRef = GetComponentInParent<SkillTree>();
        m_ButtonRef = GetComponent<Button>();
        m_ButtonRef.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        m_SkillTreeRef.SetHighlightedSkill(this);
    }
}