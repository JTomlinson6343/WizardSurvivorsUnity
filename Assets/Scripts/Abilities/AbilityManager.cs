using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager m_Instance;

    [SerializeField] List<Ability> m_Abilities;

    AbilityStats m_AbilityStatsBuffs;

    AbilityIcon[] m_Icons;

    AbilityIcon m_HighlightedIcon;

    [SerializeField] GameObject m_AbilityCanvas;

    [SerializeField] TextMeshProUGUI m_NameLabel;
    [SerializeField] TextMeshProUGUI m_DescriptionLabel;

    bool m_AbilityChoicesShown;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        //Get ability icons
        m_Icons = GetComponentsInChildren<AbilityIcon>();

        m_AbilityCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShowAbilityOptions();
        }
        if (m_AbilityChoicesShown)
        {
            HandleInput();
        }
    }

    public void ShowAbilityOptions()
    {
        if (m_Abilities.Count == 0) return;

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

        if (m_Abilities.Count < 4)
        {
            optionCount = m_Abilities.Count;
        }
        else
        {
            optionCount = 4;
        }
        // Loop through each ability
        while (count < optionCount)
        {
            Ability ability = m_Abilities[Random.Range(0, m_Abilities.Count)];
            if (ability.m_isMaxed)
            {
                // If the ability is maxed, remove it from the list and move on
                m_Abilities.Remove(ability);
                if(m_Abilities.Count < 4)
                {
                    // Reduce the number of options shown if there arent enough viable abilities
                    optionCount -= 1;
                }
                continue;
            }
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
        StateManager.TogglePause(true);
    }

    void HideAbilityOptions()
    {
        m_AbilityChoicesShown = false;

        m_AbilityCanvas.SetActive(false);

        StateManager.TogglePause(false);
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AbilityWasSelected(m_Icons[0]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AbilityWasSelected(m_Icons[1]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AbilityWasSelected(m_Icons[2]);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AbilityWasSelected(m_Icons[3]);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (m_HighlightedIcon.enabled && m_HighlightedIcon.displayedAbility != null)
            {
                // Check if icon is displayed and then enable the ability displayed
                UnlockAbility();
            }
        }
    }

    void AbilityWasSelected(AbilityIcon icon)
    {
        if (icon.displayedAbility == null) return;

        m_HighlightedIcon = icon;
        m_NameLabel.text = icon.displayedAbility.name;
        m_DescriptionLabel.text = icon.displayedAbility.name;
    }

    void UnlockAbility()
    {
        m_HighlightedIcon.displayedAbility.OnChosen();
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
    }

    public AbilityStats GetAbilityStatBuffs()
    {
        return m_AbilityStatsBuffs;
    }

    public void AddAbilityStatBuffs(AbilityStats stats)
    {
        m_AbilityStatsBuffs += stats;
        UpdateAllAbilityStats();
    }

    public void AddElementalAbilityStatBuffs(DamageType type, AbilityStats stats)
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
}
