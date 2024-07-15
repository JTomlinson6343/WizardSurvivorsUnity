using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpell : Spell
{
    [SerializeField] GameObject m_SummonPrefab;

    [SerializeField] GameObject m_SmokePrefab;
    public override void OnCast()
    {
        base.OnCast();

        GameObject summon = Instantiate(m_SummonPrefab);
        GameObject smoke = Instantiate(m_SmokePrefab);

        summon.transform.position = Player.m_Instance.transform.position;
        smoke.transform.position = summon.transform.position;

        smoke.transform.localScale = summon.transform.localScale * 2f;

        summon.GetComponent<Summon>().m_AbilitySource = this;
    }

    public override void UpdateTotalStats()
    {
        if (!AbilityManager.m_Instance) return;

        base.UpdateTotalStats();

        m_TotalStats.damage += m_BonusStats.summonDamage * m_BaseStats.damage;
    }
}
