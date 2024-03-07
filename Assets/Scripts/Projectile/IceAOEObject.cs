using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAOEObject : AOEObject
{
    bool m_AlreadySpawned;

    readonly float kMeltTime = 0.25f;
    protected override void OnTargetHit(GameObject enemy)
    {
        if (m_AlreadySpawned) return;

        Debuff debuff = new Debuff(Debuff.DebuffType.Frozen, DamageType.Frost, 0f, 1, m_AbilitySource.GetTotalStats().duration, Player.m_Instance.gameObject);

        DebuffManager.m_Instance.AddDebuff(enemy.GetComponent<Actor>(), debuff);

        DamageTarget(enemy);
    }

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        base.Init(pos, ability, lifetime);
        StartCoroutine(StopDamaging());
    }

    protected override void DestroySelf()
    {
        GetComponentInChildren<Animator>().Play("Melt", -1, 0f);

        StartCoroutine(Melt());
    }

    IEnumerator Melt()
    {
        yield return new WaitForSeconds(kMeltTime);

        Destroy(gameObject);
    }

    IEnumerator StopDamaging()
    {
        yield return new WaitForSeconds(0.1f);

        m_AlreadySpawned = true;
    }
}
