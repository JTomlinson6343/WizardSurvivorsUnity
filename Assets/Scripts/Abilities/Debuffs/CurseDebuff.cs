using System.Collections;
using UnityEngine;

public class CurseDebuff : SlowDebuff
{
    public CurseDebuff(DebuffType type, DamageType damageType, float damage, float maxStacks, float duration, GameObject source, Ability abilitySource, float slowAmount)
        : base(type, damageType, damage, maxStacks, duration, source, slowAmount) { }

    public override void OnApply(Actor actor)
    {
        base.OnApply(actor);
        // Reduce speed
        actor.GetComponent<Enemy>().m_DamageResistance -= 0.3f;
    }

    public override void OnEnd(Actor actor)
    {
        actor.GetComponent<Enemy>().m_DamageResistance += 0.3f;
    }
}
