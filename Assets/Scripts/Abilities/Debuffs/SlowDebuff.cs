using System.Collections;
using UnityEngine;

public class SlowDebuff : Debuff
{
    public float m_SlowAmount;

    public SlowDebuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source, float slowAmount)
        : base(type, damageType, damage, maxStacks, duration, source)
    {
        m_SlowAmount = slowAmount;
    }

    public SlowDebuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source, Ability abilitySource, float slowAmount)
        : this(type, damageType, damage, maxStacks, duration, source, slowAmount) { }

    public override void OnApply(Actor actor)
    {
        base.OnApply(actor);

        // Reduce speed
        actor.GetComponent<Enemy>().m_Speed *= (1f - m_SlowAmount);
        actor.GetComponentInChildren<Animator>().speed *= (1f - m_SlowAmount);
    }

    public override void OnEnd(Actor actor)
    {
        base.OnEnd(actor);

        // Increase speed back to normal
        actor.GetComponent<Enemy>().m_Speed /= (1f - m_SlowAmount);
        actor.GetComponentInChildren<Animator>().speed /= (1f - m_SlowAmount);
    }
}