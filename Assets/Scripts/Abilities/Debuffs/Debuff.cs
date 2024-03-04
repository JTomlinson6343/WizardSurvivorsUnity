using System.Collections;
using UnityEngine;

public class Debuff
{
    // Consts
    public readonly float kTickRate = 0.25f; // Ticks are every 0.25s
    private readonly float kMaxStacks;
    private readonly float kDuration;

    protected GameObject m_Source;
    public Ability m_AbilitySource;

    public DebuffType m_Type { get; }
    public DamageType m_DamageType { get; }

    public float m_Damage;
    public float m_TimeLeft;
    protected int m_StackAmount = 1;

    public Debuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source)
    {
        kDuration = duration;
        kMaxStacks = maxStacks;
        m_Type = type;
        
        m_DamageType = damageType;
        m_Damage = damage;
        m_TimeLeft = duration;
        m_Source = source;

        m_StackAmount = 1;
    }

    public Debuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source, Ability abilitySource)
        : this(type, damageType, damage, maxStacks, duration, source)
    {
        m_AbilitySource = abilitySource;
    }

    public virtual void OnApply(Actor actor) { }

    public virtual void OnTick(Actor actor)
    {
        if (m_Damage <= 0) return;
        DamageActor(actor);
    }

    public virtual void OnEnd(Actor actor) { }

    private void DamageActor(Actor actor)
    {
        // Set position of the damage numbers
        Vector2 pos = Vector2.zero;
        if (actor.m_DebuffPlacement)
        {
            pos = actor.m_DebuffPlacement.transform.position;
        }
        else
        {
            pos = actor.transform.position;
        }
        pos += new Vector2(0.5f, 1f);

        // Create damage instance event
        DamageInstanceData data = new DamageInstanceData(m_Source, actor.gameObject);
        data.amount = m_Damage * m_StackAmount;
        data.damageType = m_DamageType;
        data.isDoT = true;
        data.doDamageNumbers = true;
        data.doSoundEffect = false;
        DamageManager.m_Instance.DamageInstance(data, pos);
    }

    public void RefreshTimer()
    {
        // Increment stack if possible
        if (m_StackAmount < kMaxStacks)
        {
            m_StackAmount++;
        }

        // Refresh timer
        m_TimeLeft = m_Damage;
    }
}