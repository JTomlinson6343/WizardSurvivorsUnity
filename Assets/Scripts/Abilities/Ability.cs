using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AbilityInfo
{
    public string name;
    public string element;
    public string description;
    public Sprite icon;
}

[System.Serializable]
public struct AbilityStats
{
    public static AbilityStats operator +(AbilityStats left, AbilityStats right)
    {
        AbilityStats stats;
        stats.AOE = left.AOE + right.AOE;
        stats.duration = left.duration + right.duration;
        stats.damageScaling = left.damageScaling + right.damageScaling;
        stats.speed = left.speed + right.speed;
        stats.cooldown = left.cooldown + right.cooldown;
        stats.amount = left.amount + right.amount;
        stats.knockback =  left.amount + right.amount;
        stats.pierceAmount = left.pierceAmount + right.pierceAmount;

        return stats;
    }

    public float AOE;           // Modifier of radius
    public float duration;      // Duration in seconds
    public float damageScaling; // Percentage of player damage dealt by the ability
    public float speed;         // Speed of projectile/animation of the ability
    public float cooldown;      // Cooldown in seconds of the ability
    public int amount;        // Amount of projectiles fired by the ability
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
    protected AbilityStats m_AbilityStatsBuffs;
    protected AbilityStats m_TotalStats;  // Total combined stats combining base stats, bonus stats and ability stats from buff abilities
    public AbilityInfo     m_Info;        // Info about the ability to display on upgrad screen

    private void Start()
    {
        UpdateTotalStats();
    }

    virtual public void OnChosen()
    {
        if (!m_Enabled)
        {
            Debug.Log(m_Info.name + " was enabled.");
            m_Enabled = true;
            CancelInvoke(nameof(OnCast));
            InvokeRepeating(nameof(OnCast), 0, m_TotalStats.cooldown);
        }
        else
        {
            Debug.Log(m_Info.name + " is now level "+ m_Level.ToString());

            LevelUp();
        }
    }

    virtual public void OnCast()
    {

    }

    public void UpdateTotalStats()
    {
        // Update total stats
        m_TotalStats = m_BaseStats + m_BonusStats + AbilityManager.m_Instance.GetAbilityStatBuffs();
    }

    // Called on level up and calls a different function depending on current ability level
    public virtual void LevelUp()
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
        UpdateTotalStats();
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
