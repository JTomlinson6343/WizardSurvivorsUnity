using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] Ability[] m_Abilities;

    AbilityStats m_AbilityStatsBuffs;

    public void Start()
    {
        ShowAbilityOptions();
    }

    void ShowAbilityOptions()
    {
        Image[] icons = GetComponentsInChildren<Image>();

        icons[0].sprite = m_Abilities[0].m_Info.icon;
    }
}
