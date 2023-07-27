using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    private Skill m_CurrentSkill;

    private int m_TotalSkillPoints;
    [SerializeField] private int m_CurrentSkillPoints;

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

        // Set info labels to the info of the skill
        m_NameLabel.text = m_CurrentSkill.m_SkillName;
        m_DescriptionLabel.text = m_CurrentSkill.m_Description;
        m_CostLabel.text = m_CurrentSkill.m_Cost.ToString();

        if (!m_UnlockButton.enabled )
        {
            m_UnlockButton.enabled = true;
        }

        m_UnlockButton.interactable = true;

        if (m_CurrentSkillPoints < m_CurrentSkill.m_Cost)
        {
            m_UnlockButton.interactable = false;
        }
        if (m_CurrentSkill.m_Unlocked == true)
        {
            m_UnlockButton.interactable = false;
        }
        if (!m_CurrentSkill.CheckPrerequisites())
        {
            m_UnlockButton.interactable = false;
        }
    }

    void OnUnlockPressed()
    {
        m_CurrentSkill.m_Unlocked = true;
        m_UnlockButton.interactable = false;
    }

    private void Update()
    {
        
    }
}
