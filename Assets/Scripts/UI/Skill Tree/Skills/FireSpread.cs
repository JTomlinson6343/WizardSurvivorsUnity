using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpread : CooldownSkill
{
    [SerializeField] float m_Radius;
    [SerializeField] FireDebuffSkill m_FireDebuffSkillRef;
    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.target.GetComponent<FireDebuff>() == null) return;
        if (!damageInstance.didKill) return;

        List<GameObject> enemies = GameplayManager.GetAllEnemiesInRadius(damageInstance.target.transform.position, m_Radius);

        if (enemies.Count > 1)
            StartCooldown();

        foreach (GameObject enemy in enemies)
        {
            m_FireDebuffSkillRef.ApplyFireDebuff(damageInstance.user, enemy);
        }
    }
}