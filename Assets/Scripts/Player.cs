using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator +(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.damage = left.damage + right.damage;
        newstats.speed = left.speed + right.speed;
        newstats.fireRate = left.fireRate + right.fireRate;
        newstats.shotSpeed = left.shotSpeed + right.shotSpeed;
        newstats.maxHealth = left.maxHealth + right.maxHealth;
        newstats.healthRegen = left.maxHealth + right.maxHealth;
        return newstats;
    }

    public float damage;
    public float speed;
    public float fireRate;
    public float shotSpeed;
    public float maxHealth;
    public float healthRegen;
}

public class Player : Actor
{
    [SerializeField] GameObject staffPos;
    [SerializeField] GameObject centrePos;

    public static Player m_Instance;
    [SerializeField] PlayerStats m_BaseStats;
    PlayerStats m_BonusStats;
    PlayerStats m_TotalStats;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        m_TotalStats = m_BaseStats + m_BonusStats;
        UpdateHealth();

        BasicBar bar = gameObject.GetComponentInChildren<BasicBar>();
        bar.UpdateSize(GetHealthAsRatio());
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public PlayerStats GetStats()
    {
        return m_TotalStats;
    }

    public void AddBonusStats(PlayerStats stats)
    {
        m_BonusStats += stats;
    }

    public void UpdateHealth()
    {
        float ratio = GetHealthAsRatio();
        m_MaxHealth = m_TotalStats.maxHealth;
        m_Health = m_MaxHealth * ratio;
    }

    public Transform GetStaffTransform()
    {
        return staffPos.transform;
    }
    
    public Vector3 GetCentrePos()
    {
        return centrePos.transform.position;
    }

    public float GetFireDelay()
    {
        return 1 / (1 + m_TotalStats.fireRate);
    }
}
