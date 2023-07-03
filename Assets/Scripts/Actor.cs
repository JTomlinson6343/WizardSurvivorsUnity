using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageStats
{
    public float m_FireResist;
    public float m_FrostResist;
    public float m_LightningResist;
    public float m_PoisonResist;
    public float m_PhysicalResist;
}

public enum DamageType
{
    Fire,
    Frost,
    Lightning,
    Poison,
    Physical
}

public struct DamageInstance
{
    public DamageType type;
    public float amount;
}

public class Actor : MonoBehaviour
{
    protected float m_MaxHealth = 100.0f;
    public float m_Health = 100.0f;
    private float m_HealthRegen = 0.5f;
    private float m_IFramesTimer = 0.1f;

    private float m_LastHit = 0.0f;

    private DamageStats m_BaseResistance;
    private DamageStats m_BonusResistance;

    void Start()
    {
        m_Health = m_MaxHealth;
    }

    void Update()
    {
        m_Health += m_HealthRegen * Time.deltaTime;

        m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
    }

    public bool TakeDamage(float amount)
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastHit < m_IFramesTimer)
        {
            return false;
        }

        m_Health -= amount;
        AudioManager.m_Instance.PlaySound(0);

        m_LastHit = now;

        if (m_Health <= 0)
        {
            // If this has no hp left, destroy it
            OnDeath();
        }

        return true;
    }

    public bool TakeDamageNoIFrames(float amount)
    {
        m_Health -= amount;

        if (m_Health <= 0)
        {
            // If this has no hp left, destroy it
            OnDeath();
        }

        return true;
    }

    public float GetHealthAsRatio()
    {
        return m_Health / m_MaxHealth;
    }

    virtual protected void OnDeath()
    {
        Destroy(gameObject);
    }
}
