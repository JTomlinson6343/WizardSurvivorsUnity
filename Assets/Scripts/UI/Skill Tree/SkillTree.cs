using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    private Skill m_CurrentSkill;

    private int m_TotalSkillPoints;
    private int m_CurrentSkillPoints = 100;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_CostLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;
    [SerializeField] Button          m_UnlockButton;

    private void Start()
    {
        m_UnlockButton.onClick.AddListener(OnUnlockPressed);
    }

    public void SetHighlightedSkill(Skill skill)
    {
        m_CurrentSkill = skill;
        m_NameLabel.text = m_CurrentSkill.m_SkillName;
        m_DescriptionLabel.text = m_CurrentSkill.m_Description;
        m_CostLabel.text = m_CurrentSkill.m_Cost.ToString();
    }

    void OnUnlockPressed()
    {
        if (m_CurrentSkillPoints >= m_CurrentSkill.m_Cost)
        {
            m_CurrentSkill.enabled = false;
        }
    }

    private void Update()
    {
        
    }
}
