using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] List<Ability> m_Abilities;

    AbilityStats m_AbilityStatsBuffs;

    AbilityIcon[] m_Icons;

    bool m_AbilityChoicesShown;

    public void Start()
    {
        m_Icons = GetComponentsInChildren<AbilityIcon>();
        foreach (AbilityIcon icon in m_Icons)
        {
            icon.image.enabled = false;
        }

        ShowAbilityOptions();
    }

    public void Update()
    {
        if (m_AbilityChoicesShown)
        {

        }
    }

    void ShowAbilityOptions()
    {
        Ability[] displayedAbilities = new Ability[3];

        int iconCounter = 0;

        foreach (Ability ability in m_Abilities)
        {
            if (ability.m_isMaxed)
            {
                m_Abilities.Remove(ability);
                continue;
            }
            if (CheckAlreadyDisplayed(ability, displayedAbilities))
            {
                continue;
            }
            // If all checks pass, set the icon of the UI to the icon of the ability
            m_Icons[iconCounter].image.sprite = ability.m_Info.icon;
            m_Icons[iconCounter].image.enabled = true;
            iconCounter++;
        }
    }

    void HideAbilityOptions()
    {
        m_AbilityChoicesShown = false;
        foreach (AbilityIcon icon in m_Icons)
        {
            icon.enabled = false;
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

    void HandleInput(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.UpArrow:
                AbilityWasSelected(m_Icons[0]);
                break;
            case KeyCode.DownArrow:
                AbilityWasSelected(m_Icons[1]);
                break;
            case KeyCode.LeftArrow:
                AbilityWasSelected(m_Icons[2]);
                break;
            case KeyCode.RightArrow:
                AbilityWasSelected(m_Icons[3]);
                break;
            default:
                break;
        }
    }

    void AbilityWasSelected(AbilityIcon icon)
    {
        if (icon.enabled)
        {
            // Check if icon is displayed and then enable the ability displayed
            icon.displayedAbility.OnChosen();
            HideAbilityOptions();
        }
    }
}
