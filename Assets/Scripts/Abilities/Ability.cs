using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AbilityInfo
{
    public string name;
    public string description;
    public Sprite icon;
}

[System.Serializable]
public struct AbilityStats
{
    public float AOE;           // Modifier of radius
    public float duration;      // Durarion in seconds
    public float damageScaling; // Percentage of player damage dealt by the ability
    public float speed;         // Speed of projectile/animation of the ability
    public float cooldown;      // Cooldown in seconds of the ability
    public float amount;        // Amount of projectiles fired by the ability
    public float knockback;     // Knockback of the ability
    public int   pierceAmount;  // Number of enemies that can be pierced
}

public class Ability : MonoBehaviour
{
    protected int m_Level;                // Level of the ability

    protected bool m_Enabled = false;     // If ability is enabled, it will fire as normal

    public bool m_isMaxed;      // If an ability is max level, it won't show up in the ability selection

    public AbilityStats    m_BaseStats;   // Base stats of the ability
    protected AbilityStats m_BonusStats;  // Bonus stats gained when ability is leveled up
    protected AbilityStats m_TotalStats;  // Total combined stats combining base stats, bonus stats and ability stats from buff abilities
    public AbilityInfo     m_Info;        // Info about the ability to display on upgrad screen

    public void OnChosen()
    {
        if (!m_Enabled)
        {
            Debug.Log(m_Info.name + " was enabled.");
            m_Enabled = true;
        }
        else
        {
            Debug.Log(m_Info.name + " is now level "+ m_Level.ToString());

            LevelUp();
        }
    }

    void UpdateTotalStats(AbilityStats abilityStatsBuffs)
    {
        // Update total stats
        m_TotalStats.AOE = m_BaseStats.AOE + m_BonusStats.AOE + abilityStatsBuffs.AOE;

        m_TotalStats.duration = m_BaseStats.duration + m_BonusStats.duration + abilityStatsBuffs.duration;

        m_TotalStats.damageScaling = m_BaseStats.damageScaling + m_BonusStats.damageScaling + abilityStatsBuffs.damageScaling;

        m_TotalStats.speed = m_BaseStats.speed + m_BonusStats.speed + abilityStatsBuffs.speed;

        m_TotalStats.cooldown = m_BaseStats.cooldown + m_BonusStats.cooldown + abilityStatsBuffs.cooldown;

        m_TotalStats.amount = m_BaseStats.amount + m_BonusStats.amount + abilityStatsBuffs.amount;

        m_TotalStats.knockback = m_BaseStats.knockback + m_BonusStats.knockback + abilityStatsBuffs.knockback;

        m_TotalStats.pierceAmount = m_BaseStats.pierceAmount + m_BonusStats.pierceAmount + abilityStatsBuffs.pierceAmount;
    }

    // Called on level up and calls a different function depending on current ability level
    void LevelUp()
    {
        m_Level++;
        switch (m_Level)
        {
            case 1:
                Level2();
                break;
            case 2:
                Level3();
                break;
            case 3:
                Level4();
                break;
            case 4:
                Level5();
                m_isMaxed = true;
                break;
            default:
                break;
        }
    }

    // Functions called when the ability is upgraded to the specific level
    protected virtual void Level2()
    {

    }
    protected virtual void Level3()
    {

    }
    protected virtual void Level4()
    {

    }
    protected virtual void Level5()
    {

    }
}
