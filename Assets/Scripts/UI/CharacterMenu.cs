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

    public Color m_HighlightColour;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_InfoLabel;
    [SerializeField] TextMeshProUGUI m_StartSpellNameLabel;
    [SerializeField] TextMeshProUGUI m_StartSpellDescLabel;
    [SerializeField] TextMeshProUGUI m_UnlockConditionsLabel;

    [SerializeField] Button m_StartButtonRef;
    [SerializeField] Button m_SkillTreeButtonRef;
    [SerializeField] Button m_CustomiseButtonRef;
    [SerializeField] Button m_PlayButtonRef;

    [SerializeField] MainMenu m_MainMenuRef;

    [SerializeField] SkillTree[] m_SkillTreeRefs;

    private GameObject m_CurrentCharacter;
    private SkillTree  m_CurrentCharacterSkillTree;

    [SerializeField] CharacterIcon m_DefaultCharIcon;
    static CharacterIcon m_CurrentCharIcon;

    [SerializeField] CharacterIcon m_IceMageIcon;
    [SerializeField] CharacterIcon m_LightningMageIcon;
    [SerializeField] CharacterIcon m_NecroIcon;
    [SerializeField] CharacterIcon m_PriestIcon;

    public SkillTree m_GlobalSkillTree;

    [SerializeField] MultiMageMenu m_MultiMageMenu;

    public void Awake()
    {
        m_Instance = this;
    }

    private void Update()
    {
        UpdateInfo(m_CurrentCharIcon);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        GetComponent<Navigator>().Start();
        SetCurrentIcon(m_DefaultCharIcon);
        CheckUnlocks();
    }

    // Display info on info panel based on selected character
    public void UpdateInfo(CharacterIcon charIcon)
    {
        if (charIcon == null)
            return;

        m_NameLabel.text = charIcon.m_CharName;
        m_InfoLabel.text = charIcon.m_Description;
        m_StartSpellNameLabel.text = "Starting Spell:";
        m_StartSpellDescLabel.text = charIcon.m_StartSpellName + ": " + charIcon.m_StartSpellDesc;

        if (charIcon.m_Unlocked)
        {
            m_CurrentCharacterSkillTree = charIcon.m_SkillTree;
            m_CurrentCharacter = charIcon.m_Character;

            m_SkillTreeButtonRef.interactable = true;
            m_StartButtonRef.interactable = true;
        }
        else
        {
            m_CurrentCharacterSkillTree = null;
            m_CurrentCharacter = null;

            m_SkillTreeButtonRef.interactable = false;
            m_StartButtonRef.interactable = false;
        }
    }

    public void SetCurrentIcon(CharacterIcon charIcon)
    {
        m_CurrentCharIcon = charIcon;
        foreach (CharacterIcon icon in GetComponentsInChildren<CharacterIcon>())
        {
            icon.GetComponent<Image>().color = Color.white;
        }

        ToggleCustomiseButton(false);
        TogglePlayButton(true);
        ToggleSpellInfo(true);
        IconColourPass(charIcon);
    }

    private void IconColourPass(CharacterIcon charIcon)
    {
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
    
    public void LoadGlobalSkillTree()
    {
        gameObject.SetActive(false);

        m_GlobalSkillTree.gameObject.SetActive(true);
        m_GlobalSkillTree.GetComponent<Navigator2D>().Start();
    }

    public SkillTree[] GetSkillTreeRefs()
    {

        return m_SkillTreeRefs;
    }

    public void OnMultiMageSelected(CharacterIcon charIcon)
    {
        SetCurrentIcon(charIcon);

        ToggleCustomiseButton(true);
        TogglePlayButton(false);
        ToggleSpellInfo(false);

        m_StartButtonRef.interactable = m_MultiMageMenu.IsCustomisationValid();
    }

    private void ToggleCustomiseButton(bool on)
    {
        m_SkillTreeButtonRef.gameObject.SetActive(!on);
        m_CustomiseButtonRef.gameObject.SetActive(on);
    }
    private void TogglePlayButton(bool on)
    {
        m_PlayButtonRef.gameObject.SetActive(on);
    }

    private void ToggleSpellInfo(bool on)
    {
        m_StartSpellNameLabel.gameObject.SetActive(on);
        m_StartSpellDescLabel.gameObject.SetActive(on);
    }

    public void OnCustomisePressed()
    {
        gameObject.SetActive(false);

        m_MultiMageMenu.OpenMenu();
    }

    // Pass character information into player manager and load main scene
    public void OnStartPressed()
    {
        if (m_CurrentCharacter == null)
            return;

        PlayerManager.m_Character = m_CurrentCharacter;
        PlayerManager.m_GlobalSkillTreeRef = m_GlobalSkillTree;
        PlayerManager.m_SkillTreeRef = m_CurrentCharacterSkillTree;
        PlayerManager.m_GlobalSkillTreeRef.PassEnabledSkillsToManager(false);
        PlayerManager.m_SkillTreeRef.PassEnabledSkillsToManager(true);
        StateManager.ForceChangeState(StateManager.State.PLAYING);
        SceneManager.LoadScene("Main Scene");
    }

    public void OnBackPressed()
    {
        m_MainMenuRef.CloseMenu(gameObject);
    }

    void CheckUnlocks()
    {
        m_IceMageIcon.SetUnlockState(!UnlockManager.GetUnlockableWithName("Ice Mage").unlocked);
        m_LightningMageIcon.SetUnlockState(!UnlockManager.GetUnlockableWithName("Lightning Mage").unlocked);
        m_NecroIcon.SetUnlockState(!UnlockManager.GetUnlockableWithName("Necromancer").unlocked);
    }
}
