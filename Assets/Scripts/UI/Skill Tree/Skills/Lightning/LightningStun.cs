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

    [SerializeField] GameObject m_SparksVFX;

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
        AudioManager.m_Instance.PlaySound(23, 0.5f);

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
        Debuff debuff = new Debuff(DebuffType.Paralysed, DamageType.Lightning, 0f, 1, m_Duration, Player.m_Instance.gameObject);

        DebuffManager.m_Instance.AddDebuff(enemy.GetComponent<Actor>(), debuff);

        GameObject sparksVFX = Instantiate(m_SparksVFX);

        Vector2 enemyPos = enemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position;

        sparksVFX.transform.position = enemyPos;
    }

    void StunEnemiesInRange(GameObject centreEnemy, float radius)
    {
        Vector2 centreEnemyPos = centreEnemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position;

        List<GameObject> enemies = GameplayManager.GetAllEnemiesInRange(centreEnemyPos, radius);

        foreach (GameObject enemy in enemies)
        {
            StunEnemy(enemy);
        }
    }
}
