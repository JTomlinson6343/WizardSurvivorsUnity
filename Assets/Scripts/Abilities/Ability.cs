using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AbilityData
{
    public string name;
    public DamageType damageType;
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
        stats.damage = left.damage + right.damage;
        stats.speed = left.speed + right.speed;
        stats.cooldown = left.cooldown + right.cooldown;
        stats.amount = left.amount + right.amount;
        stats.knockback =  left.knockback + right.knockback;
        stats.pierceAmount = left.pierceAmount + right.pierceAmount;
        stats.infinitePierce = left.infinitePierce;

        return stats;
    }
    public static AbilityStats operator -(AbilityStats left, AbilityStats right)
    {
        AbilityStats stats;
        stats.AOE = left.AOE - right.AOE;
        stats.duration = left.duration - right.duration;
        stats.damage = left.damage - right.damage;
        stats.speed = left.speed - right.speed;
        stats.cooldown = left.cooldown - right.cooldown;
        stats.amount = left.amount - right.amount;
        stats.knockback =  left.knockback - right.knockback;
        stats.pierceAmount = left.pierceAmount - right.pierceAmount;
        stats.infinitePierce = left.infinitePierce;

        return stats;
    }
    public static AbilityStats operator *(AbilityStats left, AbilityStats right)
    {
        AbilityStats stats;
        stats.AOE = left.AOE * right.AOE;
        stats.duration = left.duration * right.duration;
        stats.damage = left.damage * right.damage;
        stats.speed = left.speed * right.speed;
        stats.cooldown = left.cooldown * right.cooldown;
        stats.amount = left.amount * right.amount;
        stats.knockback = left.amount * right.amount;
        stats.pierceAmount = left.pierceAmount * right.pierceAmount;
        stats.infinitePierce = left.infinitePierce;

        return stats;
    }

    public float AOE;           // Modifier of radius
    public float duration;      // Duration in seconds
    public float damage; // Percentage of player damage dealt by the ability
    public float speed;         // Speed of projectile/animation of the ability
    public float cooldown;      // Cooldown in seconds of the ability
    public int   amount;        // Amount of projectiles fired by the ability
    public float knockback;     // Knockback of the ability
    public int   pierceAmount;  // Number of enemies that can be pierced
    public bool  infinitePierce;
}

public class Ability : MonoBehaviour
{
    protected int m_Level = 1;                // Level of the ability
    [SerializeField] float m_DamageUpgradeAmount;

    protected bool m_Enabled = false;     // If ability is enabled, it will fire as normal

    [HideInInspector] public bool m_isMaxed;      // If an ability is max level, it won't show up in the ability selection

    public AbilityStats    m_BaseStats;   // Base stats of the ability
    protected AbilityStats m_BonusStats;  // Bonus stats gained when ability is leveled up
    protected AbilityStats m_AbilityStatsBuffs;
    protected AbilityStats m_TotalStats;  // Total combined stats combining base stats, bonus stats and ability stats from buff abilities
    public AbilityData     m_Data;        // Info about the ability to display on upgrad screen

    virtual public void Start()
    {
        UpdateTotalStats();
    }

    virtual public void OnChosen()
    {
        if (!m_Enabled)
        {
            Debug.Log(m_Data.name + " was enabled.");
            m_Enabled = true;
            CancelInvoke(nameof(OnCast));
            if (m_TotalStats.cooldown < 0)
            {
                OnCast();
            }
            else
            {
                InvokeRepeating(nameof(OnCast), 0, m_TotalStats.cooldown);
            }
        }
        else
        {
            LevelUp();
            Debug.Log(m_Data.name + " is now level "+ m_Level.ToString());
        }
    }

    virtual public void OnCast()
    {
        if (StateManager.GetCurrentState() != State.PLAYING) { return; }
    }

    // Override this to add behaviour to take in the mouse position
    virtual public void OnMouseInput(Vector2 aimDirection)
    {
        // No OnMouseInput behaviour defined. Defaulting to normal ability cast
        OnCast();
    }

    public void AddBonusStats(AbilityStats stats)
    {
        m_BonusStats += stats;
        UpdateTotalStats();
    }

    public AbilityStats GetBonusStats()
    {
        return m_BonusStats;
    }

    virtual public void UpdateTotalStats()
    {
        // Update total stats. Bonus stats are applied as a percentage of the base damage
        m_TotalStats = m_BaseStats + m_BonusStats*m_BaseStats + AbilityManager.m_Instance.GetAbilityStatBuffs()*m_BaseStats;
    }

    public AbilityStats GetTotalStats()
    {
        return m_TotalStats;
    }

    public void AddTempStats(AbilityStats stats)
    {
        m_BonusStats += stats;
        UpdateTotalStats();
    }

    private IEnumerator RemoveTempStats(AbilityStats stats, float duration)
    {
        yield return new WaitForSeconds(duration);

        m_BonusStats -= stats;
        UpdateTotalStats();
    }

    // Called on level up and calls a different function depending on current ability level
    public virtual void LevelUp()
    {
        m_Level++;
        if (m_Level >= 3)
        {
            m_isMaxed = true;
        }
        m_BonusStats.damage += m_DamageUpgradeAmount;
        UpdateTotalStats();
    }
}
