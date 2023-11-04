using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkillAOE : AOEObject
{
    // Percent health the ability does
    public float m_Damage;
    public Skill m_SkillSource;

    override protected void DamageEnemy(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, enemy);
        data.amount = m_Damage;
        data.damageType = DamageType.Fire;
        data.target = enemy;
        data.skillSource = m_SkillSource;
        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }
}
