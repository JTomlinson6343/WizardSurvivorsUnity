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

    private DamageStats m_BaseResistance;
    private DamageStats m_BonusResistance;

    private Material m_DefaultMaterial;
    public Material m_WhiteFlashMaterial;
    private float m_FlashTime = 0.1f;

    public virtual void Start()
    {
        m_DefaultMaterial = GetComponentInChildren<SpriteRenderer>().material;
        Init();
    }

    virtual public void Update()
    {
        m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
    }

    public virtual void Init()
    {
        m_Health = m_MaxHealth;
    }

    // If actor has i-frames, return false. Else, return true
    virtual public DamageOutput TakeDamage(float amount)
    {
        return OnDamage(amount);
    }

    // Called when a valid hit is registered
    virtual public DamageOutput OnDamage(float amount)
    {
        StartFlashing();

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

    virtual protected void StartFlashing()
    {
        GetComponentInChildren<SpriteRenderer>().material = m_WhiteFlashMaterial;
        Invoke(nameof(EndFlashing), m_FlashTime);
    }

    virtual protected void EndFlashing()
    {
        GetComponentInChildren<SpriteRenderer>().material = m_DefaultMaterial;
    }

    virtual protected void OnDeath()
    {
        Destroy(gameObject);
    }
}
