using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    public float damage;
    public float speed;
    public float maxHealth;
    public float healthRegen;
}

public class Player : Actor
{
    [SerializeField] GameObject staffPos;

    public static Player m_Instance;
    [SerializeField] PlayerStats m_BaseStats;
    PlayerStats m_BonusStats;
    PlayerStats m_TotalStats;

    private void Awake()
    {
        m_Instance = this;
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
}
