using System.Collections;
using UnityEngine;

public class SlowDebuff : Debuff
{
    public float m_SlowAmount;

    private Color m_OldColour;
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

        m_OldColour = actor.GetComponentInChildren<SpriteRenderer>().color;

        // Reduce speed
        actor.GetComponent<Enemy>().m_Speed *= (1f - m_SlowAmount);

        actor.GetComponentInChildren<SpriteRenderer>().color = Color.cyan * m_OldColour;
    }

    public override void OnEnd(Actor actor)
    {
        base.OnEnd(actor);

        // Increase speed back to normal
        actor.GetComponent<Enemy>().m_Speed /= (1f - m_SlowAmount);

        actor.GetComponentInChildren<SpriteRenderer>().color = m_OldColour;
    }
}