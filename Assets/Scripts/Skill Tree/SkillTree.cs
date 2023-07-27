using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Skill m_CurrentSkill;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;

    public void SetHighlightedSkill(Skill skill)
    {
        m_CurrentSkill = skill;
        m_NameLabel.text = m_CurrentSkill.m_SkillName;
        m_DescriptionLabel.text = m_CurrentSkill.m_Description;
    }

    public void EnableHighlightedSkill()
    {

    }

    private void Update()
    {
        
    }
}
