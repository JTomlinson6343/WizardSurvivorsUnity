using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct DamageStats
{
    public float m_FireResist;
    public float m_FrostResist;
    public float m_LightningResist;
    public float m_PoisonResist;
    public float m_PhysicalResist;
}

public enum DamageOutput
{
    validHit = 0,
    wasKilled = 1,
    invalidHit = -1
}

public class Actor : MonoBehaviour
{
    public float m_MaxHealth = 100.0f;
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

    virtual public void Update()
    {
        m_Health += m_HealthRegen * Time.deltaTime;

        m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
    }

    // If actor has i-frames, return false. Else, return true
    public DamageOutput TakeDamage(float amount)
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastHit < m_IFramesTimer)
        {
            return DamageOutput.invalidHit;
        }
        m_LastHit = now;

        return OnDamage(amount);
    }

    public DamageOutput TakeDamageNoIFrames(float amount)
    {
        return OnDamage(amount);
    }
    // Called when a valid hit is registered
    public DamageOutput OnDamage(float amount)
    {
        m_Health -= amount;

        if (m_Health <= 0)
        {
            // If this has no hp left, destroy it
            OnDeath();
            return DamageOutput.wasKilled;
        }

        return DamageOutput.validHit;
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
