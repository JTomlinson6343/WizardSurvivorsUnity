using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaySpell : Spell
{
    [SerializeField] float m_Range;
    [SerializeField] float m_LineDuration;

    [SerializeField] GameObject m_RayPrefab;

    private GameObject m_TargetedEnemy;

    public override void OnCast()
    {
        base.OnCast();

        m_TargetedEnemy = GetRandomEnemy();

        if (!m_TargetedEnemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        StartCoroutine(CastRay());
    }
    IEnumerator CastRay()
    {
        GameObject ray = Instantiate(m_RayPrefab);
        LineRenderer line = ray.GetComponent<LineRenderer>();

        line.SetPosition(0, Player.m_Instance.GetStaffTransform().position);
        line.SetPosition(1, m_TargetedEnemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position);
        line.enabled = true;

        float elapsed = 0f;
        float tickTimer = 0f;

        while (elapsed < m_LineDuration)
        {
            line.SetPosition(0, Player.m_Instance.GetStaffTransform().position);
            if (!m_TargetedEnemy) Destroy(ray);
            line.SetPosition(1, m_TargetedEnemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position);

            elapsed += Time.deltaTime;
            tickTimer += Time.deltaTime;
            if (tickTimer >= 0.25f)
            {
                //Damage enemy
                DamageTarget(m_TargetedEnemy);
                tickTimer = 0f;
            }
            yield return new WaitForEndOfFrame();
        }

        line.enabled = false;
        Destroy(ray);
    }

    virtual protected void DamageTarget(GameObject target)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, target);
        data.amount = GetTotalStats().damage;
        data.damageType = m_Data.damageType;
        data.target = target;
        data.abilitySource = this;
        data.doDamageNumbers = true;
        DamageManager.m_Instance.DamageInstance(data, m_TargetedEnemy.GetComponent<Enemy>().m_DebuffPlacement.transform.position);
    }

    private GameObject GetRandomEnemy()
    {
        List<GameObject> enemies = Utils.GetAllEnemiesInRange(Player.m_Instance.transform.position, m_Range);

        if (enemies.Count <= 0) return null;

        int RandNo = Random.Range(0, enemies.Count);

        return enemies[RandNo];
    }
}
