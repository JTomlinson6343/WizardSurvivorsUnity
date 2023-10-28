using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PlayerStats
{
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator +(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed + right.speed;
        newstats.shotSpeed = left.shotSpeed + right.shotSpeed;
        newstats.maxHealth = left.maxHealth + right.maxHealth;
        newstats.healthRegen = left.healthRegen + right.healthRegen;
        return newstats;
    }
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator -(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed - right.speed;
        newstats.shotSpeed = left.shotSpeed - right.shotSpeed;
        newstats.maxHealth = left.maxHealth - right.maxHealth;
        newstats.healthRegen = left.healthRegen - right.healthRegen;
        return newstats;
    }

    public static PlayerStats operator *(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed * right.speed;
        newstats.shotSpeed = left.shotSpeed * right.shotSpeed;
        newstats.maxHealth = left.maxHealth * right.maxHealth;
        newstats.healthRegen = left.healthRegen * right.healthRegen;
        return newstats;
    }
    public float speed;
    public float shotSpeed;
    public float maxHealth;
    public float healthRegen;

    public static PlayerStats Zero = new PlayerStats();
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

    [SerializeField] Ability m_ActiveAbility;

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

            if (now - m_LastShot > m_ActiveAbility.GetTotalStats().cooldown)
            {
                m_ActiveAbility.OnMouseInput(GetAimDirection().normalized);
                m_LastShot = now;
            }
        }

        UpdateStats();
    }

    protected override void OnDeath()
    {
        SceneManager.LoadScene("Menu");
        base.OnDeath();
    }
    #region Stats Functions
    public void UpdateStats()
    {
        m_TotalStats = m_BaseStats + m_BonusStats;
        UpdateHealth();

        BasicBar bar = gameObject.GetComponentInChildren<BasicBar>();
        bar.UpdateSize(GetHealthAsRatio());
    }
    public void AddBonusStats(PlayerStats stats)
    {
        m_BonusStats += stats;
    }

    public void AddTempStats(PlayerStats stats, float duration)
    {
        m_BonusStats += stats;
        //In Start() or wherever
        StartCoroutine(RemoveTempStats(stats, duration));
    }

    private IEnumerator RemoveTempStats(PlayerStats stats, float duration)
    {
        yield return new WaitForSeconds(duration);

        m_BonusStats -= stats;
    }
    #endregion

    #region Getters
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
    #endregion
}