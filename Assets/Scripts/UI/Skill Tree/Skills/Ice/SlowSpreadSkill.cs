using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSpreadSkill : CooldownSkill
{
    [SerializeField] float m_Radius;
    [SerializeField] IceSlowSkill m_IceSlowSkillRef;
    [SerializeField] GameObject m_LineCreator;
    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (DebuffManager.GetDebuffIfPresent(damageInstance.target.GetComponent<Actor>(), Debuff.DebuffType.Frostbite) == null) return;
        if (!damageInstance.didKill) return;

        List<GameObject> enemies = Utils.GetAllEnemiesInRange(damageInstance.target.transform.position, m_Radius);

        if (enemies.Count > 1)
            StartCooldown();

        foreach (GameObject enemy in enemies)
        {
            GameObject line = Instantiate(m_LineCreator);
            line.GetComponent<LineCreator>().Init(
                damageInstance.target.GetComponent<Actor>().m_DebuffPlacement.transform.position,
                enemy.transform.position,
                0.1f,
                Color.white,
                Color.cyan
                );

            m_IceSlowSkillRef.ApplySlowDebuff(damageInstance.user, enemy);
        }
    }
}