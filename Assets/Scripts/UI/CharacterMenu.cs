using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour
{
    public static CharacterMenu m_Instance;

    [SerializeField] TextMeshProUGUI m_InfoLabel;

    [SerializeField] MainMenu m_MainMenuRef;

    private SkillTree m_CurrentCharacterSkillTree;

    static CharacterIcon m_CurrentCharIcon;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Update()
    {
        UpdateInfo(m_CurrentCharIcon);
    }

    public void UpdateInfo(CharacterIcon charIcon)
    {
        if (charIcon == null)
            return;

        m_InfoLabel.text = charIcon.m_CharName + "\n\n" + charIcon.m_Description;

        m_CurrentCharacterSkillTree = charIcon.m_SkillTree;
    }

    public void SetCurrentIcon(CharacterIcon charIcon)
    {
        m_CurrentCharIcon = charIcon;
    }

    public void LoadSkillTree()
    {
        if (m_CurrentCharacterSkillTree == null)
            return;

        gameObject.SetActive(false);

        m_CurrentCharacterSkillTree.gameObject.SetActive(true);
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void OnBackPressed()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
    }
}
