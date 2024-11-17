using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiMageMenu : MonoBehaviour
{
    public static MultiMageMenu m_Instance;
    public MultiMageCharacterPanel m_LeftCharacterPanel;
    public MultiMageCharacterPanel m_RightCharacterPanel;

    public CombinedSkillTree m_CombinedTree;

    /*
     * 1. Player selects an icon, left or right.
     * 2. The icon darkens the screen and pops up a new panel that lets the player select a character.
     * 3. The icon from 1. is then replaced with the selected character.
     * 4. The skill tree button becomes visible below the icon, allowing the player to open it and spend skill points.
     * 
     * The total skill points are displayed in the middle.
     */

    private void Awake()
    {
        m_Instance = this;
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        CharacterMenu.m_Instance.gameObject.SetActive(true);
        CharacterMenu.m_Instance.gameObject.GetComponent<CharacterMenuNavigator>().Start();
        SaveManager.SaveToFile();
    }

    public void OpenSkillTree(MultiMageCharacterPanel panel)
    {
        SkillTree tree = panel.m_SelectedIcon.m_SkillTree;
        gameObject.SetActive(false);

        tree.gameObject.SetActive(true);
        tree.GetComponent<Navigator2D>().Start();
        tree.Start();
    }

    public void OnStartPressed()
    {
        if (!m_LeftCharacterPanel.m_SelectedIcon || !m_RightCharacterPanel.m_SelectedIcon) return;

        GenerateCombinedSkillTree();

        PlayerManager.m_Character = m_LeftCharacterPanel.m_SelectedIcon.m_Character;
        PlayerManager.m_MultiMageRightCharacterActiveAbilityName = m_RightCharacterPanel.m_SelectedIcon.m_Character.GetComponentInChildren<Player>().m_ActiveAbility.m_Data.name;
        PlayerManager.m_GlobalSkillTreeRef = CharacterMenu.m_Instance.m_GlobalSkillTree;
        PlayerManager.m_SkillTreeRef = m_CombinedTree;
        PlayerManager.m_GlobalSkillTreeRef.PassEnabledSkillsToManager(false);
        PlayerManager.m_SkillTreeRef.PassEnabledSkillsToManager(true);
        PlayerManager.m_DoSpawnMultiMage = true;
        StateManager.ForceChangeState(StateManager.State.PLAYING);
        SceneManager.LoadScene("Main Scene");
    }

    public void SwapCharacters()
    {
        (m_LeftCharacterPanel.m_SelectedIcon, m_RightCharacterPanel.m_SelectedIcon) = (m_RightCharacterPanel.m_SelectedIcon, m_LeftCharacterPanel.m_SelectedIcon);
        m_LeftCharacterPanel.CloseMenu();
        m_RightCharacterPanel.CloseMenu();
    }

    // Checks if two options have been selected for the multi mage
    public bool IsCustomisationValid()
    {
        return m_LeftCharacterPanel.m_SelectedIcon && m_RightCharacterPanel.m_SelectedIcon;
    }

    public void GenerateCombinedSkillTree()
    {
        m_CombinedTree.SetSkillTrees(
            m_LeftCharacterPanel.m_SelectedIcon.m_SkillTree,
            m_RightCharacterPanel.m_SelectedIcon.m_SkillTree
        );
    }
}
