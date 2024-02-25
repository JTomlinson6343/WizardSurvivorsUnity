using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAftershock : CooldownSkill
{
    [SerializeField] float m_Lifetime;
    private Ability m_Ability;
    [SerializeField] GameObject m_LightningBoltPrefab;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
        GetComponent<Ability>().UpdateTotalStats();
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Lightning) return;
        if (!damageInstance.didKill) return;
        if (GameplayManager.GetAllEnemiesInRange(damageInstance.target.transform.position, Lightning.kBaseRange * GetComponent<Ability>().GetTotalStats().AOE).Count < 1) return;

        GameObject bolt = Instantiate(m_LightningBoltPrefab);
        bolt.GetComponent<ParentLightningBolt>().Init(damageInstance.target.transform.position, GetComponent<Ability>(), m_Lifetime);
        bolt.GetComponent<ParentLightningBolt>().m_JumpLimit = 0;

        switch (m_Data.level)
        {
            case 2:
                m_Cooldown = 2f;
                break;
            default:
                break;
        }
        StartCooldown();
    }
}