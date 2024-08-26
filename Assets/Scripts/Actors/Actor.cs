using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    public enum DamageOutput
    {
        validHit = 0,
        wasKilled = 1,
        invalidHit = -1
    }

    public float m_MaxHealth = 100.0f;
    public float m_Health = 100.0f;

    public float m_KnockbackResist;

    public float m_DamageResistance;

    private Material m_DefaultMaterial;
    public Material m_WhiteFlashMaterial;
    private readonly float m_FlashTime = 0.1f;

    public GameObject m_DebuffPlacement;
    public List<Debuff> m_Debuffs = new();
    public Debuff.DebuffType[] m_DebuffImmunities;
    public bool m_DebuffImmune = false;
    public bool m_Targetable = true;

    protected bool m_IsMidAnimation;
    bool m_IsDead;
    public bool m_Stunned;

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
        if (m_IsDead) return DamageOutput.invalidHit;
        StartFlashing();

        m_Health -= amount;

        if (m_Health <= 0)
        {
            // If this has no hp left, destroy it
            m_Health = 0;
            OnDeath();
            m_IsDead = true;
            return DamageOutput.wasKilled;
        }

        return DamageOutput.validHit;
    }

    public float GetHealthAsRatio()
    {
        return m_Health / m_MaxHealth;
    }

    // Flips the actor in the direction of the their velocity
    protected void FaceVelocity(Transform transform, Vector3 targetVelocity, bool reverse)
    {
        if (reverse)
        {
            transform.localScale = new Vector3(
                targetVelocity.x > 0 ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            return;
        }

        transform.localScale = new Vector3(
            targetVelocity.x > 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    // Called whenever this actor is knocked back
    virtual public void KnockbackRoutine(Vector2 dir, float knockbackMagnitude)
    {
        knockbackMagnitude = Mathf.Clamp01(1f - m_KnockbackResist) * knockbackMagnitude;
        GetComponent<Rigidbody2D>().velocity += dir.normalized * knockbackMagnitude;
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

    virtual public void ToggleStunned(bool enabled)
    {
        m_Stunned = enabled;

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        if (!rb) return;

        // Disable animator if stunned, enable if not stunned
        GetComponentInChildren<Animator>().enabled = !enabled;

        if (enabled)
        {
            // Freeze movement
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            // Unfreeze movement
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void ClearDebuffs()
    {
        foreach (Debuff debuff in m_Debuffs)
        {
            debuff.EndEarly();
        }
    }

    // Method called after 'delay' seconds and only if it is off cooldown
    protected void PlayMethodAfterAnimation(string animation, float delay, string methodOnPlay, ref bool cooldownCheck)
    {
        Animator animator = GetComponentInChildren<Animator>();

        if (cooldownCheck) return;

        // Play the animation
        animator.Play(animation, -1, 0f);
        // Set status to being mid-animation
        m_IsMidAnimation = true;
        // Call the method after the delay
        Invoke(methodOnPlay, delay);
        cooldownCheck = true;
    }
    protected void PlayMethodAfterAnimation(string animation, float delay, string methodOnPlay)
    {
        Animator animator = GetComponentInChildren<Animator>();

        // Play the animation
        animator.Play(animation, -1, 0f);
        // Set status to being mid-animation
        m_IsMidAnimation = true;
        // Call the method after the delay
        Invoke(methodOnPlay, delay);
    }
    protected void PlayMethodAfterAnimation(string animation, float delay, Action method)
    {
        Animator animator = GetComponentInChildren<Animator>();

        // Play the animation
        animator.Play(animation, -1, 0f);
        // Set status to being mid-animation
        m_IsMidAnimation = true;
        // Call the method after the delay
        StartCoroutine(Utils.DelayedCall(delay, method));
    }
}
