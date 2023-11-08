using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    private SkillIcon m_CurrentSkill;

    [SerializeField] private int m_TotalSkillPoints;
    private int m_SkillPointCap;
    private int m_CurrentSkillPoints;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_CostLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;
    [SerializeField] TextMeshProUGUI m_OnLevelUpLabel;
    [SerializeField] TextMeshProUGUI m_CantUnlockLabel;
    [SerializeField] TextMeshProUGUI m_SkillLevelLabel;
    [SerializeField] TextMeshProUGUI m_SkillPointsLabel;

    [SerializeField] Button          m_UnlockButton;
    [SerializeField] Button          m_RespecButton;
    [SerializeField] Button          m_BackButton;

    [SerializeField] string m_NotEnoughSkillPointsMsg;
    [SerializeField] string m_SkillMaxedMsg;
    [SerializeField] string m_PrereqMsg;

    private void Start()
    {
        m_UnlockButton.onClick.AddListener(OnUnlockPressed);
        m_RespecButton.onClick.AddListener(OnRespecPressed);
        m_BackButton.onClick.AddListener(OnBackPressed);

        m_CurrentSkillPoints = m_TotalSkillPoints;

        UpdateSkillPointsLabel();
        GreyOrWhitePass();
    }

    private void UpdateSkillPointsLabel()
    {
        m_SkillPointsLabel.text = "SP: " + m_CurrentSkillPoints.ToString() + "/" + m_TotalSkillPoints.ToString();
    }

    private int GetCurrentLevelCost()
    {
        if (m_CurrentSkill.m_Cost.Length <= m_CurrentSkill.m_Data.level)
            return -1;
        else
            return m_CurrentSkill.m_Cost[m_CurrentSkill.m_Data.level];

    }

    private string GetCurrentOnLevelUpMessage()
    {
        if (m_CurrentSkill.m_Cost.Length <= m_CurrentSkill.m_Data.level || m_CurrentSkill.m_Data.level == 0)
            return "";
        else
            return "Next level: " + m_CurrentSkill.m_OnLevelUpDescription[m_CurrentSkill.m_Data.level-1];
    }

    public void SetHighlightedSkill(SkillIcon skill)
    {
        m_CurrentSkill = skill;

        // Set info labels to the info of the skill
        m_NameLabel.text = m_CurrentSkill.m_SkillName;

        string description = "";
        // If the current level is greater than the number of descriptions, use the last description.
        if (m_CurrentSkill.m_Data.level > m_CurrentSkill.m_Description.Length-1)
            description = m_CurrentSkill.m_Description.Last();
        else
            description = m_CurrentSkill.m_Description[m_CurrentSkill.m_Data.level];
        m_DescriptionLabel.text = description;

        m_OnLevelUpLabel.text = GetCurrentOnLevelUpMessage();
        m_CostLabel.text = "Cost: " + GetCurrentLevelCost().ToString();

        // Display unlock button
        m_UnlockButton.gameObject.SetActive(true);
        m_UnlockButton.interactable = true;
        m_CantUnlockLabel.text = "";

        CheckSelectedSkill();
    }

    void CheckSelectedSkill()
    {
        if (m_CurrentSkill == null)
            return;

        // Unlock button won't work unless it passes these checks
        if (m_CurrentSkill.IsMaxed())
        {
            m_UnlockButton.gameObject.SetActive(false);
            m_CantUnlockLabel.text = m_SkillMaxedMsg;
            m_CostLabel.text = "";
        }
        if (m_CurrentSkillPoints < GetCurrentLevelCost())
        {
            m_UnlockButton.interactable = false;
            m_CantUnlockLabel.text = m_NotEnoughSkillPointsMsg;
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
        m_CurrentSkill.Unlock();
        m_CurrentSkillPoints -= GetCurrentLevelCost();
        UpdateSkillPointsLabel();
        m_CurrentSkill.m_Data.level++;
        // Ability unlock animation goes here

        SetHighlightedSkill(m_CurrentSkill);
        GreyOrWhitePass();
    }

    void OnRespecPressed()
    {
        SkillIcon[] skills = GetComponentsInChildren<SkillIcon>();

        foreach (SkillIcon skill in skills)
        {
            skill.Lock();
            skill.m_Data.level = 0;
        }

        m_CurrentSkillPoints = m_TotalSkillPoints;
        UpdateSkillPointsLabel();
        SetHighlightedSkill(m_CurrentSkill);
        GreyOrWhitePass();
    }

    void GreyOrWhitePass()
    {
        SkillIcon[] skills = GetComponentsInChildren<SkillIcon>();

        foreach (SkillIcon skill in skills)
        {
            skill.GreyOrWhite();
        }
    }

    void OnBackPressed()
    {
        OnCloseSkillTreeMenu();
        gameObject.SetActive(false);
        CharacterMenu.m_Instance.gameObject.SetActive(true);
    }

    void PassEnabledSkillsToManager()
    {
        SkillIcon[] skills = GetComponentsInChildren<SkillIcon>();

        SkillManager.m_Instance.ResetSkillsAdded();

        foreach (SkillIcon skillIcon in skills)
        {
            if (!skillIcon.m_Unlocked) continue;

            SkillManager.m_Instance.AddSkill(skillIcon.m_Data);

            Debug.Log(skillIcon.m_SkillName + "added");
        }
    }

    void OnCloseSkillTreeMenu()
    {
        PassEnabledSkillsToManager();
    }
}
