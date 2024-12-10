using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestAOESkill : CooldownSkill
{
    [SerializeField] float m_Damage;
    [SerializeField] float m_Radius;
    [SerializeField] GameObject m_SunbeamEffectPrefab;
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
        if (damageInstance.damageType != DamageType.Light) return;
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
                DamageManager.GetDamageNumberColor(DamageType.Light),
                Color.white
                );

            GameObject sunBeam = Instantiate(m_SunbeamEffectPrefab);
            sunBeam.transform.position = enemy.transform.position;
            DamageTarget(enemy);
        }
    }

    private void DamageTarget(GameObject target)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, target);
        data.amount = m_Damage;
        data.damageType = DamageType.Light;
        data.target = target;
        data.doDamageNumbers = true;
        DamageManager.m_Instance.DamageInstance(data, target.GetComponent<Enemy>().m_DebuffPlacement.transform.position);
    }
}