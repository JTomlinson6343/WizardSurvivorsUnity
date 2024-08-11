using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    const float kCooldownCap = -0.7f;

    public static AbilityManager m_Instance;

    [SerializeField] List<Ability> m_PassiveAbilities;
    [SerializeField] List<Ability> m_BuffAbilities;

    AbilityStats m_AbilityStatsBuffs;

    AbilityIcon[] m_Icons;

    AbilityIcon m_HighlightedIcon;

    [SerializeField] GameObject m_AbilityCanvas;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;
    [SerializeField] TextMeshProUGUI m_InstructionsLabel;

    [SerializeField] string m_SpellInstructions;
    [SerializeField] string m_ItemInstructions;

    bool m_AbilityChoicesShown;

    public bool m_LightningDoubleCastOn;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        //Get ability icons
        m_Icons = GetComponentsInChildren<AbilityIcon>();

        m_AbilityCanvas.SetActive(false);

        CheckUnlocks(m_PassiveAbilities);
        CheckUnlocks(m_BuffAbilities);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    ShowAbilityOptions(m_PassiveAbilities);
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ShowAbilityOptions(m_BuffAbilities.FindAll(a => a.m_Unlocked));
        //}
        if (m_AbilityChoicesShown)
        {
            HandleInput();
        }
    }

    private void CheckUnlocks(List<Ability> abilities)
    {
        foreach (Ability ability in abilities)
        {
            Unlockable unlock = UnlockManager.GetUnlockableWithName(ability.m_Data.name);

            if (unlock == null) continue;

            ability.m_Unlocked = unlock.unlocked;
        }
    }

    // Displays 4 spells for the player to choose
    public void ChoosePassiveAbility()
    {
        ShowAbilityOptions(m_PassiveAbilities.FindAll(a => a.m_Unlocked));
        m_InstructionsLabel.text = m_SpellInstructions;
    }
    // Displays 4 items for the player to choose
    public void ChooseBuffAbility()
    {
        ShowAbilityOptions(m_BuffAbilities.FindAll(a => a.m_Unlocked));
        m_InstructionsLabel.text = m_ItemInstructions;
    }

    private void ShowAbilityOptions(List<Ability> abilities)
    {
        if (abilities.Count == 0) return;

        // Reset info panel
        m_NameLabel.text = "";
        m_DescriptionLabel.text = "";

        m_AbilityCanvas.SetActive(true);
        foreach (AbilityIcon icon in m_Icons)
        {
            icon.image.enabled = false;
        }

        m_AbilityChoicesShown = true;
        Ability[] displayedAbilities = new Ability[4];

        int iconCounter = 0;

        int count = 0;

        int optionCount;

        if (abilities.Count < 4)
        {
            optionCount = abilities.Count;
        }
        else
        {
            optionCount = 4;
        }
        // Loop through each ability
        while (count < optionCount)
        {
            Ability ability = abilities[Random.Range(0, abilities.Count)];

            if (CheckAlreadyDisplayed(ability, displayedAbilities))
            {
                //If the ability is already displayed on the other choices, move on
                continue;
            }
            displayedAbilities[count] = ability;
            // If all checks pass, set the icon of the UI to the icon of the ability
            m_Icons[iconCounter].image.sprite = ability.m_Data.icon;
            // Set the ability the icon represents to that ability
            m_Icons[iconCounter].displayedAbility = ability;
            // Show the icon
            m_Icons[iconCounter].image.enabled = true;

            AudioManager.m_Instance.PlaySound(7);

            iconCounter++;

            count++;
        }
        ProgressionManager.m_Instance.ToggleHUD(false);
        StateManager.ChangeState(StateManager.State.UPGRADING);
    }

    void HideAbilityOptions()
    {
        m_AbilityChoicesShown = false;

        m_AbilityCanvas.SetActive(false);

        ProgressionManager.m_Instance.ToggleHUD(true);

        StateManager.UnPause();

        DeHighlightAbilityIcons();
    }

    bool CheckAlreadyDisplayed(Ability ability, Ability[] displayedAbilities)
    {
        // Check if ability is already displayed
        foreach (Ability displayedAbility in displayedAbilities)
        {
            if (ability == displayedAbility)
            {
                // If ability is already shown, test fails
                return true;
            }
        }
        return false;
    }

    void HandleInput()
    {
        if (StateManager.GetCurrentState() != StateManager.State.UPGRADING) return;

        if (Input.GetAxis("VerticalDPAD") > 0f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            AbilityWasSelected(m_Icons[0]);
        }
        else if (Input.GetAxis("VerticalDPAD") < 0f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            AbilityWasSelected(m_Icons[1]);
        }
        else if (Input.GetAxis("HorizontalDPAD") < 0f || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AbilityWasSelected(m_Icons[2]);
        }
        else if (Input.GetAxis("HorizontalDPAD") > 0f || Input.GetKeyDown(KeyCode.RightArrow))
        {
            AbilityWasSelected(m_Icons[3]);
        }
        if (Input.GetButtonDown("Submit")) {
            if (m_HighlightedIcon.image.enabled && m_HighlightedIcon.displayedAbility != null)
            {
                // Check if icon is displayed and then enable the ability displayed
                UnlockAbility();
            }
        }
    }

    // Called whenever an ability is highlighted
    public void AbilityWasSelected(AbilityIcon icon)
    {
        if (!icon.image.enabled) return;

        m_HighlightedIcon = icon;
        DeHighlightAbilityIcons();
        icon.GetComponent<Image>().color = Color.yellow;
        m_NameLabel.text = icon.displayedAbility.m_Data.name;
        if (icon.displayedAbility.m_IsSpell) m_DescriptionLabel.text = icon.displayedAbility.m_Data.description;
        else
        {
            m_NameLabel.text += " " + Utils.IntToRomanNumeral(icon.displayedAbility.GetLevel() + 1);
            m_DescriptionLabel.text = "";
            if (icon.displayedAbility.GetLevel() > 0) m_DescriptionLabel.text = "Next level:\n";
            m_DescriptionLabel.text += icon.displayedAbility.m_Data.levelUpInfo;
        }
    }

    private void DeHighlightAbilityIcons()
    {
        foreach (AbilityIcon otherIcon in m_Icons)
        {
            otherIcon.GetComponent<Image>().color = Color.white;
        }
    }

    // Unlocks the currently highlighted ability
    void UnlockAbility()
    {
        m_HighlightedIcon.displayedAbility.OnChosen();
        if (m_HighlightedIcon.displayedAbility.m_isMaxed)
        {
            m_PassiveAbilities.Remove(m_HighlightedIcon.displayedAbility);
            m_BuffAbilities.Remove(m_HighlightedIcon.displayedAbility);
        }
        HideAbilityOptions();

        UpdateAllAbilityStats();
        m_HighlightedIcon = null;
    }

    private void UpdateAllAbilityStats()
    {
        Ability[] abilities = GetComponentsInChildren<Ability>();

        // Update ability stats
        foreach (Ability ability in abilities)
        {
            ability.UpdateTotalStats();
        }

        if (m_LightningDoubleCastOn) UpdateDoubleCastedSpell();

        UnlockManager.GetTrackedStatWithName("totalCooldown").stat = GetAbilityStatBuffs().cooldown;
        UnlockManager.CheckUnlockConditions();
    }

    public AbilityStats GetAbilityStatBuffs()
    {
        return m_AbilityStatsBuffs;
    }

    public void AddAbilityStatBuffs(AbilityStats stats)
    {
        m_AbilityStatsBuffs += stats;
        if (m_AbilityStatsBuffs.cooldown < kCooldownCap) m_AbilityStatsBuffs.cooldown = kCooldownCap;
        m_AbilityStatsBuffs.pierceAmount += stats.pierceAmount;
        UpdateAllAbilityStats();
    }

    public void AddElementalAbilityBonusStats(DamageType type, AbilityStats stats)
    {
        Ability[] abilities = GetComponentsInChildren<Ability>();

        // Update ability stats
        foreach (Ability ability in abilities)
        {
            // Add bonus stats to abilities with same damage type
            if (ability.m_Data.damageType == type || type == DamageType.None)
                ability.AddBonusStats(stats);
        }
        UpdateAllAbilityStats();
    }

    public void AddTempElementalAbilityStatBuffs(DamageType type, AbilityStats stats, float duration)
    {
        Ability[] abilities = GetComponentsInChildren<Ability>();

        // Update ability stats
        foreach (Ability ability in abilities)
        {
            // Add bonus stats to abilities with same damage type
            if (ability.m_Data.damageType == type || type == DamageType.None)
                ability.AddTempStats(stats, duration);
        }
        UpdateAllAbilityStats();
    }

    public void AddTempAbilityStatBuffs(AbilityStats stats, float duration)
    {
        Ability[] abilities = GetComponentsInChildren<Ability>();

        // Update ability stats
        foreach (Ability ability in abilities)
        {
            // Add bonus stats to abilities
            ability.AddTempStats(stats, duration);
        }
        UpdateAllAbilityStats();
    }

    public Spell[] GetAllSpells()
    {
        return GetComponentsInChildren<Spell>();
    }

    public Spell GetHighestCooldownSpell()
    {
        Spell highestSpell = GetAllSpells()[0];

        foreach (Spell spell in GetAllSpells())
        {
            if (!spell.IsEnabled()) continue;
            if (!spell.HasTag(Spell.SpellTag.Offensive)) continue;
            if (spell.GetTotalStats().cooldown > highestSpell.GetTotalStats().cooldown) highestSpell = spell;
        }

        return highestSpell;
    }

    private void UpdateDoubleCastedSpell()
    {
        foreach (Spell spell in GetComponentsInChildren<Spell>())
        {
            spell.m_CastAmount = spell.m_BaseCastAmount;
        }

        GetHighestCooldownSpell().m_CastAmount = GetHighestCooldownSpell().m_BaseCastAmount * 2;
    }
}
