using System.Collections;
using UnityEngine;

public class Item : Ability
{
    [SerializeField] AbilityStats m_AbilityStatBuffs;
    [SerializeField] PlayerStats m_PlayerStatBuffs;

    public override void LevelUp()
    {
        AbilityManager.m_Instance.AddAbilityStatBuffs(m_AbilityStatBuffs);
        Player.m_Instance.AddBonusStats(m_PlayerStatBuffs);

        if (m_PlayerStatBuffs.maxHealth > 0)
        {
            Player.m_Instance.IncreaseMaxHealth(m_PlayerStatBuffs.maxHealth);
            Player.m_Instance.Heal(m_PlayerStatBuffs.maxHealth * 3f);
        }
        base.LevelUp();
    }

    public override void OnChosen()
    {
        if (!m_Enabled)
        {
            Debug.Log(m_Data.name + " was enabled.");
            m_Enabled = true;
        }
        LevelUp();
        Debug.Log(m_Data.name + " is now level " + m_Level.ToString());
    }
}