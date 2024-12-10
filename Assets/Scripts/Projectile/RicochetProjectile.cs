using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RicochetProjectile : Projectile
{
    public GameObject m_TargetEnemy;
    public GameObject m_TriggeringEnemy;
    public Skill m_SkillSource;

    protected override void OnTargetHit(GameObject target)
    {
        //if (target == m_TriggeringEnemy) return;
        if (target != m_TargetEnemy) return;
        
        base.OnTargetHit(target);
    }

    protected override void DamageTarget(GameObject target)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, target);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Data.damageType;
        data.target = target;
        data.abilitySource = m_AbilitySource;
        data.hitPosition = transform.position;
        data.skillSource = m_SkillSource;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }
}
