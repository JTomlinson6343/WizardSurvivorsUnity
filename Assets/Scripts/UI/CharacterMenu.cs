using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public static CharacterMenu m_Instance;

    [SerializeField] TextMeshProUGUI m_InfoLabel;

    [SerializeField] GameObject m_Buttons;

    [SerializeField] MainMenu m_MainMenuRef;

    private GameObject m_CurrentCharacter;
    private SkillTree  m_CurrentCharacterSkillTree;

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
        m_CurrentCharacter = charIcon.m_Character;

        m_Buttons.SetActive(true);

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
        if (m_CurrentCharacter == null)
            return;

        PlayerSpawner.m_Character = m_CurrentCharacter;
        StateManager.ChangeState(State.PLAYING);
        SceneManager.LoadScene("Main Scene");
    }

    public void OnBackPressed()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
    }
}
