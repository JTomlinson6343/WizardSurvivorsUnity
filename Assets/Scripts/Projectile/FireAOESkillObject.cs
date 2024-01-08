using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class FireAOESkillObject : AOEObject
{
    [HideInInspector] public float m_Damage; // percent max hp
    [HideInInspector] public Skill m_SkillSource;
    [HideInInspector] public FireDebuffSkill m_FireDebuffSkillRef;

    override protected void DamageTarget(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, enemy);
        data.amount = m_Damage * enemy.GetComponent<Enemy>().m_MaxHealth;
        data.damageType = DamageType.Fire;
        data.target = enemy;
        data.skillSource = m_SkillSource;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }

    public void Init(Vector2 pos, float damage, float scale, float lifetime)
    {
        transform.localScale *= scale;
        GetComponent<Projectile>().StartLifetimeTimer(lifetime);
        transform.position = pos;
    }
}
