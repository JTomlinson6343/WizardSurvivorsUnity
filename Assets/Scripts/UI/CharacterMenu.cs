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

    [SerializeField] Color m_HighlightColour;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_InfoLabel;
    [SerializeField] TextMeshProUGUI m_UnlockConditionsLabel;

    [SerializeField] Button m_StartButtonRef;
    [SerializeField] Button m_SkillTreeButtonRef;

    [SerializeField] MainMenu m_MainMenuRef;

    SkillTree[] m_SkillTreeRefs;

    private GameObject m_CurrentCharacter;
    private SkillTree  m_CurrentCharacterSkillTree;

    [SerializeField] CharacterIcon m_DefaultCharIcon;
    static CharacterIcon m_CurrentCharIcon;

    [SerializeField] CharacterIcon m_IceMageIcon;
    [SerializeField] CharacterIcon m_LightningMageIcon;

    public void Awake()
    {
        m_Instance = this;

        SetCurrentIcon(m_DefaultCharIcon);
        CheckUnlocks();
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

        m_NameLabel.text = charIcon.m_CharName;
        m_InfoLabel.text = charIcon.m_Description;

        if (charIcon.m_Unlocked)
        {
            m_CurrentCharacterSkillTree = charIcon.m_SkillTree;
            m_CurrentCharacter = charIcon.m_Character;

            m_SkillTreeButtonRef.interactable = true;
            m_StartButtonRef.interactable = true;

            m_UnlockConditionsLabel.text = "";
        }
        else
        {
            m_CurrentCharacterSkillTree = null;
            m_CurrentCharacter = null;

            m_SkillTreeButtonRef.interactable = false;
            m_StartButtonRef.interactable = false;

            string message = "";
            if (charIcon == m_IceMageIcon) message = UnlockManager.GetUnlockConditionWithName("Ice Mage").FormatConditionMessage(UnlockManager.m_TrackedStats.iceDamageDealt);
            if (charIcon == m_LightningMageIcon) message = UnlockManager.GetUnlockConditionWithName("Lightning Mage").message;

            m_UnlockConditionsLabel.text = message;
        }
    }

    public void SetCurrentIcon(CharacterIcon charIcon)
    {
        m_CurrentCharIcon = charIcon;
        foreach (CharacterIcon icon in GetComponentsInChildren<CharacterIcon>())
        {
            icon.GetComponent<Image>().color = Color.white;
        }

        CheckUnlocks();

        charIcon.GetComponent<Image>().color = m_HighlightColour;
        if (!charIcon.m_Unlocked) charIcon.GetComponent<Image>().color *= Color.grey;
    }

    public void LoadSkillTree()
    {
        if (m_CurrentCharacterSkillTree == null)
            return;

        gameObject.SetActive(false);

        m_CurrentCharacterSkillTree.gameObject.SetActive(true);
        m_CurrentCharacterSkillTree.GetComponent<Navigator2D>().Start();
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
        StateManager.ForceChangeState(StateManager.State.PLAYING);
        SceneManager.LoadScene("Main Scene");
    }

    public void OnBackPressed()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
        m_MainMenuRef.GetComponent<Navigator>().Start();
    }

    void CheckUnlocks()
    {
        m_IceMageIcon.SetUnlockState(!UnlockManager.GetUnlockableWithName("Ice Mage").unlocked);
        m_LightningMageIcon.SetUnlockState(!UnlockManager.GetUnlockableWithName("Lightning Mage").unlocked);
    }
}
