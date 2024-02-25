using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LightningStun : CooldownSkill
{
    [SerializeField] float m_Duration;
    [SerializeField] float m_Radius;
    [SerializeField] float m_RadiusIncrease;

    [SerializeField] GameObject m_LinePrefab;
    [SerializeField] float m_LineDuration;

    override public void Init(SkillData data)
    {
        base.Init(data);
        DamageManager.m_DamageInstanceEvent.AddListener(OnDamageInstance);
    }

    public void OnDamageInstance(DamageInstanceData damageInstance)
    {
        if (m_OnCooldown) return;
        if (!damageInstance.user.CompareTag("Player")) return;
        if (damageInstance.damageType != DamageType.Lightning) return;

        StartCooldown();

        switch (m_Data.level)
        {
            case 1:
                StunEnemy(damageInstance.target);
                break;
            case 2:
                StunEnemiesInRange(damageInstance.target, m_Radius);
                break;
            case 3:
                StunEnemiesInRange(damageInstance.target, m_Radius * m_RadiusIncrease);
                break;
            default:
                break;
        }
    }

    void StunEnemy(GameObject enemy)
    {
        FrozenDebuff stunDebuff = enemy.AddComponent<FrozenDebuff>();
        stunDebuff.Init(m_Duration, 0, DamageType.None, gameObject, false, 1, DebuffType.Paralysed);
    }

    void StunEnemiesInRange(GameObject centreEnemy, float radius)
    {
        Vector2 centreEnemyPos = centreEnemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position;

        List<GameObject> enemies = GameplayManager.GetAllEnemiesInRange(centreEnemy.transform.position, radius);

        foreach (GameObject enemy in enemies)
        {
            Vector2 enemyPos = enemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position;

            GameObject line = Instantiate(m_LinePrefab);
            line.GetComponent<LineCreator>().Init(centreEnemyPos, enemyPos, m_LineDuration,Color.white,Color.blue);
            StunEnemy(enemy);
        }
    }
}
