using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] List<Ability> m_Abilities;

    AbilityStats m_AbilityStatsBuffs;

    public void Start()
    {
        ShowAbilityOptions();
    }

    void ShowAbilityOptions()
    {
        Ability[] displayedAbilities = new Ability[3];

        Image[] icons = GetComponentsInChildren<Image>();

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
            icons[iconCounter].sprite = ability.m_Info.icon;
        }
        icons[0].sprite = m_Abilities[0].m_Info.icon;
    }
    
    bool CheckAlreadyDisplayed(Ability ability, Ability[] displayedAbilities)
    {
        // Check if ability is already displayed
        foreach (Ability displayedAbility in displayedAbilities)
        {
            if (ability == displayedAbility)
            {
                // If ability is not already shown, test fails
                return true;
            }
        }
        return false;
    }
}
