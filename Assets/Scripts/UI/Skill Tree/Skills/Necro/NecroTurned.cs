using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroTurned : Skill
{
    [SerializeField] float m_Chance = 0.01f;

    public override void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;
        if (!damageInstance.didKill) return;

        TrySpawnSkeleton(damageInstance.target);
    }

    public void TrySpawnSkeleton(GameObject target)
    {
        if (Random.Range(0f, 1f) >= m_Chance) return;

        RaiseDead.SpawnSkeleton(target.transform.position);
    }
}
