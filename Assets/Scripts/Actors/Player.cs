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
        newstats.speed = left.speed + right.speed;
        newstats.fireRate = left.fireRate + right.fireRate;
        newstats.shotSpeed = left.shotSpeed + right.shotSpeed;
        newstats.maxHealth = left.maxHealth + right.maxHealth;
        newstats.healthRegen = left.healthRegen + right.healthRegen;
        return newstats;
    }

    public float speed;
    public float fireRate;
    public float shotSpeed;
    public float maxHealth;
    public float healthRegen;
}

public class Player : Actor
{
    [SerializeField] Camera m_CameraRef;

    [SerializeField] GameObject staffPos;
    [SerializeField] GameObject centrePos;

    [SerializeField] GameObject m_DamageNumberPrefab;

    public static Player m_Instance;
    [SerializeField] PlayerStats m_BaseStats;
    PlayerStats m_BonusStats;
    PlayerStats m_TotalStats;

    [SerializeField] Ability m_BasicAbility;

    float m_LastShot = 0;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        UpdateStats();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(0))
        {
            float now = Time.realtimeSinceStartup;

            if (now - m_LastShot > GetFireDelay())
            {
                m_BasicAbility.OnCast();
                m_LastShot = now;
            }
        }
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

    public Vector2 GetAimDirection()
    {
        return (m_CameraRef.ScreenToWorldPoint(Input.mousePosition) - GetStaffTransform().position).normalized;

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
        return 1 / (0.1f + m_TotalStats.fireRate);
    }
}
