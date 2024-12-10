using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestRicochet : Skill
{
    [SerializeField] float m_Radius;
    [SerializeField] float m_Chance;
    [SerializeField] GameObject m_RicochetShotPrefab;

    Ability m_RadiantBoltRef;

    public override void Init(SkillData data)
    {
        base.Init(data);
        m_Chance *= m_Data.level;
        m_RadiantBoltRef = AbilityManager.m_Instance.GetAbilityWithName("Radiant Bolt");
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.abilitySource != m_RadiantBoltRef) return;
        if (damageInstance.skillSource == this) return;
        if (Random.Range(0f, 1f) >= m_Chance) return;

        Vector2 pos = damageInstance.hitPosition;

        GameObject closestEnemy = Utils.GetFurthestEnemyInRange(pos, m_Radius);

        Vector2 dir = Utils.GetDirectionToGameObject(pos, closestEnemy);

        GameObject bullet = ProjectileManager.m_Instance.Shoot(pos,
            dir,
            m_RadiantBoltRef.GetTotalStats().speed, m_RadiantBoltRef, m_RadiantBoltRef.GetComponent<Firebolt>().m_ProjectileLifetime, m_RicochetShotPrefab);
        RicochetProjectile ricochetComponent = bullet.GetComponent<RicochetProjectile>();
        ricochetComponent.m_TriggeringEnemy = damageInstance.target;
        ricochetComponent.m_TargetEnemy = closestEnemy;
        ricochetComponent.m_SkillSource = this;
    }
}
