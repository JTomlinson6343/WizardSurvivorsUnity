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

public class Actor : MonoBehaviour
{
    private float m_MaxHealth = 100.0f;
    private float m_Health = 100.0f;
    private float m_HealthRegen = 0.0f;

    private DamageStats m_BaseResistance;
    private DamageStats m_BonusResistance;

    void Awake()
    {
        m_Health = m_MaxHealth;
    }

    void Update()
    {
        m_Health += m_HealthRegen * Time.deltaTime;

        m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);
    }

    public void TakeDamage(float amount)
    {
        m_Health -= amount;
    }
}
