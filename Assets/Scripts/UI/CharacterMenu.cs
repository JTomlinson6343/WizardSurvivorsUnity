using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public static CharacterMenu m_Instance;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_InfoLabel;

    [SerializeField] MainMenu m_MainMenuRef;

    SkillTree[] m_SkillTreeRefs;

    private GameObject m_CurrentCharacter;
    private SkillTree  m_CurrentCharacterSkillTree;

    [SerializeField] CharacterIcon m_DefaultCharIcon;
    static CharacterIcon m_CurrentCharIcon;

    private void Awake()
    {
        m_Instance = this;

        m_CurrentCharIcon = m_DefaultCharIcon;
    }

    private void Start()
    {


        //SaveManager.LoadFromFile();
    }

    private void Update()
    {
        UpdateInfo(m_CurrentCharIcon);
    }

    // Display info on info panel based on selected character
    public void UpdateInfo(CharacterIcon charIcon)
    {
        if (charIcon == null)
            return;

        HighlightSelectedIcon(charIcon);

        m_NameLabel.text = charIcon.m_CharName;
        m_InfoLabel.text = charIcon.m_Description;

        m_CurrentCharacterSkillTree = charIcon.m_SkillTree;
        m_CurrentCharacter = charIcon.m_Character;
    }

    private void HighlightSelectedIcon(CharacterIcon charIcon)
    {
        foreach (CharacterIcon icon in GetComponentsInChildren<CharacterIcon>())
        {
            icon.GetComponent<Image>().color = Color.white;
        }
        charIcon.GetComponent<Image>().color = Color.yellow;
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

    public SkillTree[] GetSkillTreeRefs()
    {
        m_SkillTreeRefs = new SkillTree[GetComponentsInChildren<CharacterIcon>().Length];

        for (int i = 0; i < GetComponentsInChildren<CharacterIcon>().Length; i++)
        {
            m_SkillTreeRefs[i] = GetComponentsInChildren<CharacterIcon>()[i].m_SkillTree;
        }

        return m_SkillTreeRefs;
    }

    // Pass character information into player manager and load main scene
    public void OnStartPressed()
    {
        if (m_CurrentCharacter == null)
            return;

        PlayerManager.m_Character = m_CurrentCharacter;
        PlayerManager.m_SkillTreeRef = m_CurrentCharacterSkillTree;
        PlayerManager.m_SkillTreeRef.PassEnabledSkillsToManager();
        StateManager.ForceChangeState(State.PLAYING);
        SceneManager.LoadScene("Main Scene");
    }

    public void OnBackPressed()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
    }
}
