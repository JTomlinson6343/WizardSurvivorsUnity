using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AbilityData
{
    public string name;
    public DamageType damageType;
    [TextArea(3, 10)]
    public string description;
    [TextArea(3, 10)]
    public string levelUpInfo;
    public Sprite icon;
}

[System.Serializable]
public struct AbilityStats
{
    // Operator functions for adding ability stats together.
    public static AbilityStats operator +(AbilityStats left, AbilityStats right)
    {
        AbilityStats stats;
        stats.AOE = left.AOE + right.AOE;
        stats.duration = left.duration + right.duration;
        stats.damage = left.damage + right.damage;
        stats.summonDamage = left.summonDamage + right.summonDamage;
        stats.speed = left.speed + right.speed;
        stats.cooldown = left.cooldown + right.cooldown;
        stats.castAmount = left.castAmount + right.castAmount;
        stats.knockback =  left.knockback + right.knockback;
        stats.critChance =  left.critChance + right.critChance;
        stats.pierceAmount = left.pierceAmount;
        stats.infinitePierce = left.infinitePierce;
        stats.neverPierce = left.neverPierce;

        return stats;
    }
    public static AbilityStats operator -(AbilityStats left, AbilityStats right)
    {
        AbilityStats stats;
        stats.AOE = left.AOE - right.AOE;
        stats.duration = left.duration - right.duration;
        stats.summonDamage = left.summonDamage - right.summonDamage;
        stats.damage = left.damage - right.damage;
        stats.speed = left.speed - right.speed;
        stats.cooldown = left.cooldown - right.cooldown;
        stats.castAmount = left.castAmount - right.castAmount;
        stats.knockback =  left.knockback - right.knockback;
        stats.critChance = left.critChance - right.critChance;
        stats.pierceAmount = left.pierceAmount;
        stats.infinitePierce = left.infinitePierce;
        stats.neverPierce = left.neverPierce;

        return stats;
    }
    public static AbilityStats operator *(AbilityStats left, AbilityStats right)
    {
        AbilityStats stats;
        stats.AOE = left.AOE * right.AOE;
        stats.duration = left.duration * right.duration;
        stats.damage = left.damage * right.damage;
        stats.summonDamage = left.summonDamage * right.summonDamage;
        stats.speed = left.speed * right.speed;
        stats.cooldown = left.cooldown * right.cooldown;
        stats.castAmount = left.castAmount * right.castAmount;
        stats.knockback = left.knockback * right.knockback;
        stats.critChance = left.critChance * right.critChance;
        stats.pierceAmount = left.pierceAmount;
        stats.infinitePierce = left.infinitePierce;
        stats.neverPierce = left.neverPierce;

        return stats;
    }

    public float AOE;           // Modifier of radius
    public float duration;      // Duration in seconds
    public float damage; // Percentage of player damage dealt by the ability
    public float summonDamage;
    public float speed;         // Speed of projectile/animation of the ability
    public float cooldown;      // Cooldown in seconds of the ability
    public float knockback;     // Knockback of the ability
    public int   pierceAmount;  // Number of enemies that can be pierced
    public bool  infinitePierce;
    public bool  neverPierce;
    public float critChance;
    public int   castAmount;
}

public class Ability : MonoBehaviour
{
    protected int m_Level = 0;                // Level of the ability

    protected bool m_Enabled = false;     // If ability is enabled, it will fire as normal

    public bool m_Unlocked = true;

    [HideInInspector] public bool m_isMaxed;      // If an ability is max level, it won't show up in the ability selection

    public bool m_IsSpell;

    public int     m_CastAmount = 1;
    public int     m_BaseCastAmount = 1;
    public float   multiCastDelay = 0.3f;

    public AbilityStats    m_BaseStats;   // Base stats of the ability
    protected AbilityStats m_BonusStats;  // Bonus stats gained when ability is leveled up
    protected AbilityStats m_AbilityStatsBuffs;
    protected AbilityStats m_TotalStats;  // Total combined stats combining base stats, bonus stats and ability stats from buff abilities
    public AbilityData     m_Data;        // Info about the ability to display on upgrade screen

    public float m_CooldownRemaining;

    private List<GameObject> m_HitEnemies = new List<GameObject>();
    private List<Coroutine> m_CooldownCoroutines = new List<Coroutine>();

    public float m_DefaultAutofireRange = 10f;
    protected readonly float kCooldownAfterReset = 1f;
    protected float kMinCooldownModifier = 0.1f;

    //Getters//
    public AbilityStats GetBonusStats() { return m_BonusStats; }
    public AbilityStats GetTotalStats() { return m_TotalStats; }

    virtual public void Start()
    {
        UpdateTotalStats();
    }

    private void Awake()
    {
        UpdateTotalStats();
    }

    // Called when the ability is chosen on the ability menu
    virtual public void OnChosen()
    {
        if (!m_Enabled)
        {
            //Debug.Log(m_Data.name + " was enabled.");
            m_Enabled = true;
            CastSpell();
            StartCoroutine(CooldownRoutine());
        }
        LevelUp();
        //Debug.Log(m_Data.name + " is now level " + m_Level.ToString());
    }

    protected virtual IEnumerator CooldownRoutine()
    {
        if (m_TotalStats.cooldown < 0f) yield break;

        while (true) // Repeating
        {
            while (m_CooldownRemaining > 0f) // Count down cooldown
            {
                m_CooldownRemaining -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            CastSpell();
        }
    }

    private void CastSpell()
    {
        m_CooldownRemaining = m_TotalStats.cooldown;
        StartCoroutine(MultiCast());
    }

    protected void SetRemainingCooldown(float newCooldown)
    {
        m_CooldownRemaining = newCooldown;
    }

    private IEnumerator MultiCast()
    {
        for (int i = 0; i < m_CastAmount; i++)
        {
            OnCast();

            yield return new WaitForSeconds(multiCastDelay);
        }
    }

    public virtual void ToggleAutofire(bool on)
    {
        if (on)
        {
            SetRemainingCooldown(m_TotalStats.cooldown);
            m_CooldownCoroutines.Add(StartCoroutine(CooldownRoutine()));
        }
        else
        {
            int count = 0;
            while (m_CooldownCoroutines.Count > 0 && count < 1000)
            {
                StopCoroutine(m_CooldownCoroutines[count]);
                count++;
            }
            m_CooldownCoroutines.Clear();
        }
    }

    // Called every time the cooldown ends
    virtual public void OnCast()
    {
        if (StateManager.GetCurrentState() != StateManager.State.PLAYING) { return; }
    }

    // Override this to add behaviour to take in the mouse position
    virtual public void OnMouseInput(Vector2 aimDirection)
    {
        // No OnMouseInput behaviour defined. Defaulting to normal ability cast
        OnCast();
    }

    #region Stats
    public void AddBonusStats(AbilityStats stats)
    {
        m_BonusStats += stats;
        UpdateTotalStats();
    }

    virtual public void UpdateTotalStats()
    {
        if (!AbilityManager.m_Instance) return;

        // Update total stats. Bonus stats are applied as a percentage of the base damage
        m_TotalStats = m_BaseStats + m_BonusStats*m_BaseStats + AbilityManager.m_Instance.GetAbilityStatBuffs()*m_BaseStats;
        if (m_TotalStats.cooldown <= 0)
            m_TotalStats.cooldown = m_BaseStats.cooldown * kMinCooldownModifier;

        m_TotalStats.pierceAmount = m_BaseStats.pierceAmount + AbilityManager.m_Instance.GetAbilityStatBuffs().pierceAmount;
        m_CastAmount = m_BonusStats.castAmount + m_BaseCastAmount;
    }

    public void AddTempStats(AbilityStats stats, float duration)
    {
        m_BonusStats += stats;
        UpdateTotalStats();
        StartCoroutine(RemoveTempStats(stats, duration));
    }

    private IEnumerator RemoveTempStats(AbilityStats stats, float duration)
    {
        yield return new WaitForSeconds(duration);

        m_BonusStats -= stats;
        UpdateTotalStats();
    }
    #endregion

    // Called on level up and calls a different function depending on current ability level
    public virtual void LevelUp()
    {
        m_Level++;
        if (m_Level >= 3)
        {
            m_isMaxed = true;
        }
        UpdateTotalStats();
    }

    public int GetLevel() { return m_Level; }

    // Cooldown to prevent the same enemy from being affected by this ability too often.
    public void StartDamageCooldown(GameObject enemy)
    {
        m_HitEnemies.Add(enemy);
        StartCoroutine(EndDamageCooldown(enemy));
    }

    private IEnumerator EndDamageCooldown(GameObject enemy)
    {
        yield return new WaitForSeconds(0.1f);

        m_HitEnemies.Remove(enemy);
    }

    public bool DamageOnCooldownCheck(GameObject enemy)
    {
        if (m_HitEnemies.Contains(enemy)) return true;

        return false;
    }

    public bool IsEnabled()
    {
        return m_Enabled;
    }
}
