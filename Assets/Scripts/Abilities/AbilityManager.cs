using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager m_Instance;

    [SerializeField] List<Ability> m_Abilities;

    AbilityStats m_AbilityStatsBuffs;

    AbilityIcon[] m_Icons;

    bool m_AbilityChoicesShown;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        //Get ability icons
        m_Icons = GetComponentsInChildren<AbilityIcon>();

        HideAbilityOptions();
    }

    private void Update()
    {
        if (m_AbilityChoicesShown)
        {
            HandleInput();
        }
    }

    public void ShowAbilityOptions()
    {
        m_AbilityChoicesShown = true;
        Ability[] displayedAbilities = new Ability[3];

        int iconCounter = 0;

        // Loop through each ability
        foreach (Ability ability in m_Abilities)
        {
            if (ability.m_isMaxed)
            {
                // If the ability is maxed, remove it from the list and move on
                m_Abilities.Remove(ability);
                continue;
            }
            if (CheckAlreadyDisplayed(ability, displayedAbilities))
            {
                //If the ability is already displayed on the other choices, move on
                continue;
            }
            // If all checks pass, set the icon of the UI to the icon of the ability
            m_Icons[iconCounter].image.sprite = ability.m_Info.icon;
            // Set the ability the icon represents to that ability
            m_Icons[iconCounter].displayedAbility = ability;
            // Show the icon
            m_Icons[iconCounter].image.enabled = true;

            iconCounter++;
        }
    }

    void HideAbilityOptions()
    {
        m_AbilityChoicesShown = false;

        foreach (AbilityIcon icon in m_Icons)
        {
            icon.image.enabled = false;
        }
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
    }

    void AbilityWasSelected(AbilityIcon icon)
    {
        if (icon.enabled)
        {
            // Check if icon is displayed and then enable the ability displayed
            icon.displayedAbility.OnChosen();
            HideAbilityOptions();
            Player.m_Instance.UpdateStats();
        }
    }

    public AbilityStats GetAbilityStatBuffs()
    {
        return m_AbilityStatsBuffs;
    }
}
