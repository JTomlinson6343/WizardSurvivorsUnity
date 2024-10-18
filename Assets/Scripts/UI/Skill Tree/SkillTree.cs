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

    public Color m_CharacterColour;
    public string m_CharacterName;

    public int m_TotalSkillPoints;
    public static readonly int kSkillPointCap = 999;
    public int m_CurrentSkillPoints;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_CostLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;
    [SerializeField] TextMeshProUGUI m_OnLevelUpLabel;
    [SerializeField] TextMeshProUGUI m_CantUnlockLabel;
    [SerializeField] TextMeshProUGUI m_SkillLevelLabel;
    [SerializeField] TextMeshProUGUI m_SkillPointsLabel;
    [SerializeField] GameObject m_SkillTreePanel;

    [SerializeField] Button          m_UnlockButton;
    [SerializeField] Button          m_RespecButton;
    [SerializeField] Button          m_BackButton;

    [SerializeField] string m_NotEnoughSkillPointsMsg;
    [SerializeField] string m_SkillMaxedMsg;
    [SerializeField] string m_PrereqMsg;

    private void Start()
    {
        UpdateSkillPointsLabel();
        ColorCheckPass();
        ColourAllIcons();

        if (m_TotalSkillPoints > 0) TutorialManager.DisplayTutorial("Skill Trees");
    }

    private void ColourAllIcons()
    {
        foreach (SkillIcon icon in GetComponentsInChildren<SkillIcon>())
        {
            icon.m_Color = m_CharacterColour;
        }
    }

    public SkillIcon GetSkillIconWithID(SkillID id)
    {
        foreach (SkillIcon icon in GetComponentsInChildren<SkillIcon>())
        {
            if (icon.m_Data.id == id) return icon;
        }

        return null;
    }

    private void UpdateSkillPointsLabel()
    {
        m_SkillPointsLabel.text = "Current: " + m_CurrentSkillPoints.ToString();
    }

    private int GetCurrentLevelCost()
    {
        // If the number of costs defined is less/equal to the current level of the skill, return -1. If this happens, it is due to human error.
        if (m_CurrentSkill.m_Cost.Length <= m_CurrentSkill.m_Data.level)
            return -1;
        else
            return m_CurrentSkill.m_Cost[m_CurrentSkill.m_Data.level];
    }

    // Message displayed for what happens when leveling up the skill
    private string GetCurrentOnLevelUpMessage()
    {
        if (m_CurrentSkill.m_Cost.Length <= m_CurrentSkill.m_Data.level || m_CurrentSkill.m_Data.level == 0)
            return "";
        else
            if (m_CurrentSkill.m_OnLevelUpDescription.Length < m_CurrentSkill.m_Data.level - 1)
                return "Next level: " + m_CurrentSkill.m_OnLevelUpDescription.Last();
            else 
                return "Next level: " + m_CurrentSkill.m_OnLevelUpDescription[m_CurrentSkill.m_Data.level-1];
    }

    public void SetHighlightedSkill(SkillIcon skill)
    {
        m_CurrentSkill = skill;

        // Set info labels to the info of the skill
        if (!m_CurrentSkill)
        {
            ClearInfoScreen();
            return;
        }

        m_NameLabel.text = m_CurrentSkill.m_SkillName;
        // If the current level is greater than the number of descriptions, use the last description.
        if (m_CurrentSkill.m_Data.level > m_CurrentSkill.m_Description.Length - 1)
            m_DescriptionLabel.text = m_CurrentSkill.m_Description.Last();
        else
        {
            if (m_CurrentSkill.m_Data.level - 1 < 0)
                m_DescriptionLabel.text = m_CurrentSkill.m_Description[0];
            else
                m_DescriptionLabel.text = m_CurrentSkill.m_Description[m_CurrentSkill.m_Data.level-1];
        }
        m_OnLevelUpLabel.text = GetCurrentOnLevelUpMessage();

        string colour1 = m_CurrentSkillPoints < GetCurrentLevelCost() ? "<color=red>" : "";
        string colour2 = m_CurrentSkillPoints < GetCurrentLevelCost() ? "</color>" : "";
        m_CostLabel.text = "Cost: " + colour1 + GetCurrentLevelCost().ToString() + colour2;

        // Display unlock button
        m_UnlockButton.gameObject.SetActive(true);
        m_UnlockButton.interactable = true;
        m_CantUnlockLabel.text = "";

        CheckSelectedSkill();
    }

    private void ClearInfoScreen()
    {
        m_NameLabel.text = "";
        m_DescriptionLabel.text = "";
        m_OnLevelUpLabel.text = "";
        m_CantUnlockLabel.text = "";
        m_CostLabel.text = "";
    }

    void CheckSelectedSkill()
    {
        if (m_CurrentSkill == null)
        {
            m_UnlockButton.interactable = false;
            return;
        }

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
            m_CostLabel.text = "Cost: " + m_CurrentSkill.m_Cost[0].ToString();
        }
    }

    public void OnUnlockPressed()
    {
        m_CurrentSkill.Unlock();
        m_CurrentSkillPoints -= GetCurrentLevelCost();
        UpdateSkillPointsLabel();
        m_CurrentSkill.LevelUp();
        m_CurrentSkill.SetLevelIndicator();

        // Ability unlock animation goes here

        SetHighlightedSkill(m_CurrentSkill);
        ColorCheckPass();
        TryUnlockAchievement();
    }

    public void OnRespecPressed()
    {
        SkillIcon[] skills = GetComponentsInChildren<SkillIcon>();

        // Re-lock all skills and set their levels to 0
        foreach (SkillIcon skill in skills)
        {
            skill.Lock();
            skill.m_Data.level = 0;
        }

        // Refund all spent skill points.
        m_CurrentSkillPoints = m_TotalSkillPoints;

        // Reset the skill tree visuals
        UpdateSkillPointsLabel();
        SetHighlightedSkill(null);
        ColorCheckPass();
        GetComponent<Navigator2D>().Start();
    }

    // Called every time an an ability is unlocked or reset.
    void ColorCheckPass()
    {
        SkillIcon[] skills = GetComponentsInChildren<SkillIcon>();

        foreach (SkillIcon skill in skills)
        {
            skill.ColorCheck();
        }
    }

    bool AllSkillsMaxedCheck()
    {
        bool areMaxed = true;
        foreach (SkillIcon skill in m_SkillTreePanel.GetComponentsInChildren<SkillIcon>())
        {
            if (!skill.IsMaxed()) areMaxed = false;
        }
        return areMaxed;
    }

    public void TryUnlockAchievement()
    {
        if (AllSkillsMaxedCheck() && m_CharacterName != null && m_CharacterName != "")
            UnlockManager.GetAchievementWithName("Maxed_" + m_CharacterName).Unlock();
    }

    // Return to character menu
    public void OnBackPressed()
    {
        OnCloseSkillTreeMenu();
        gameObject.SetActive(false);
        CharacterMenu.m_Instance.gameObject.SetActive(true);
        CharacterMenu.m_Instance.gameObject.GetComponent<CharacterMenuNavigator>().Start();

    }

    // Enable certain skills in skill manager in the main game depending on which skills are enabled on the menu
    public void PassEnabledSkillsToManager(bool dontClear)
    {
        SkillIcon[] skills = GetComponentsInChildren<SkillIcon>();

        if (!dontClear) SkillManager.m_Instance.ResetSkillsAdded();

        foreach (SkillIcon skillIcon in skills)
        {
            if (!skillIcon.m_Unlocked) continue;

            SkillManager.m_Instance.AddSkill(skillIcon.m_Data);

            Debug.Log(skillIcon.m_SkillName + "added");
        }
    }

    void OnCloseSkillTreeMenu()
    {
        SaveManager.SaveToFile(false);
    }
}
