using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAOEObject : AOEObject
{
    bool m_AlreadySpawned;
    protected override void OnTargetHit(GameObject enemy)
    {
        if (m_AlreadySpawned) return;

        FrozenDebuff frozenDebuff = enemy.AddComponent<FrozenDebuff>();

        frozenDebuff.Init(
            m_AbilitySource.GetTotalStats().duration,
            1f,
            DamageType.Frost,
            gameObject,
            false,
            1,
            DebuffType.Frozen
            );

        DamageTarget(enemy);
    }

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        base.Init(pos, ability, lifetime);
        StartCoroutine(StopDamaging());
    }

    IEnumerator StopDamaging()
    {
        yield return new WaitForSeconds(0.1f);

        m_AlreadySpawned = true;
    }
}
