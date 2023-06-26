using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
        newstats.shotSpeed = left.shotSpeed + right.shotSpeed;
        newstats.maxHealth = left.maxHealth + right.maxHealth;
        newstats.healthRegen = left.maxHealth + right.maxHealth;
        return newstats;
    }
    public float damage;
    public float speed;
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
        Player.m_Instance.UpdateStats();
    }

    public void UpdateStats()
    {
        m_TotalStats = m_BaseStats + m_BonusStats;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public PlayerStats GetStats()
    {
        return m_TotalStats;
    }

    public Transform GetStaffTransform()
    {
        return staffPos.transform;
    }
    
    public Vector3 GetCentrePos()
    {
        return centrePos.transform.position;
    }
}
