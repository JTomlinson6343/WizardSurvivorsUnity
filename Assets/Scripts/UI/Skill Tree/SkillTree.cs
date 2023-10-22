using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    private Skill m_CurrentSkill;

    [SerializeField] private int m_TotalSkillPoints;
    private int m_SkillPointCap;
    private int m_CurrentSkillPoints;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_CostLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;
    [SerializeField] TextMeshProUGUI m_CantUnlockLabel;
    [SerializeField] TextMeshProUGUI m_SkillLevelLabel;
    [SerializeField] TextMeshProUGUI m_SkillPointsLabel;

    [SerializeField] Button          m_UnlockButton;
    [SerializeField] Button          m_RespecButton;

    [SerializeField] string m_NotEnoughSkillPointsMsg;
    [SerializeField] string m_PrereqMsg;

    private void Start()
    {
        m_UnlockButton.onClick.AddListener(OnUnlockPressed);

        m_CurrentSkillPoints = m_TotalSkillPoints;

        UpdateSkillPointsLabel();
    }

    private void UpdateSkillPointsLabel()
    {
        m_SkillPointsLabel.text = "SP: " + m_CurrentSkillPoints.ToString() + "/" + m_TotalSkillPoints.ToString();
    }

    public void SetHighlightedSkill(Skill skill)
    {
        m_CurrentSkill = skill;

        // Set info labels to the info of the skill
        m_NameLabel.text = m_CurrentSkill.m_SkillName;
        m_DescriptionLabel.text = m_CurrentSkill.m_Description;
        m_CostLabel.text = m_CurrentSkill.m_Cost.ToString();

        // Display unlock button
        m_UnlockButton.gameObject.SetActive(true);
        m_UnlockButton.interactable = true;
        m_CantUnlockLabel.text = "";

        // Unlock button won't work unless it passes these checks
        if (m_CurrentSkillPoints < m_CurrentSkill.m_Cost)
        {
            m_UnlockButton.interactable = false;
            m_CantUnlockLabel.text = m_NotEnoughSkillPointsMsg;
        }
        if (m_CurrentSkill.m_Unlocked == true)
        {
            m_UnlockButton.gameObject.SetActive(false);
            m_CostLabel.text = "";
        }
        if (!m_CurrentSkill.CheckPrerequisites())
        {
            m_UnlockButton.interactable = false;
            m_CantUnlockLabel.text = m_PrereqMsg;
            m_CostLabel.text = "";
        }
    }

    void OnUnlockPressed()
    {
        m_CurrentSkillPoints -= m_CurrentSkill.m_Cost;
        UpdateSkillPointsLabel();
        m_CurrentSkill.m_SkillLevel++;
        // Ability unlock animation goes here

        if (m_CurrentSkill.m_SkillLevel < m_CurrentSkill.m_MaxSkillLevel)
            return;

        m_CurrentSkill.m_Unlocked = true;
        m_UnlockButton.interactable = false;
        m_UnlockButton.gameObject.SetActive(false);
        m_CostLabel.text = "";
    }

    private void Update()
    {
        
    }
}
