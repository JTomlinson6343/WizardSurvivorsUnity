using System.Collections;
using UnityEngine;

public class NecroBleedDebuff : Debuff
{
    public float m_Resistance = 0.1f;

    public NecroBleedDebuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source, float resistance)
        : base(type, damageType, damage, maxStacks, duration, source)
    {
        m_Resistance = resistance;
    }

    public NecroBleedDebuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source, Ability abilitySource, float resistance)
        : this(type, damageType, damage, maxStacks, duration, source, resistance) { }

    public override void OnApply(Actor actor)
    {
        base.OnApply(actor);
        // Increade damage reduction
        actor.GetComponent<Actor>().m_DamageResistance += m_Resistance;
    }

    public override void OnEnd(Actor actor)
    {
        base.OnEnd(actor);
        actor.GetComponent<Actor>().m_DamageResistance -= m_Resistance;
    }
}
