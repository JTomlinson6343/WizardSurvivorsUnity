using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Ability
{
    GameObject m_AOEObject;
    [SerializeField] GameObject m_Fog;
    [SerializeField] GameObject m_Snow;
    [SerializeField] GameObject m_BlizzardPrefab;
    public override void OnCast()
    {
        base.OnCast();
        m_AOEObject = Instantiate(m_BlizzardPrefab);
        m_AOEObject.transform.localScale *= m_TotalStats.AOE;
        m_AOEObject.transform.SetParent(Player.m_Instance.gameObject.transform);
        m_AOEObject.transform.position = Player.m_Instance.GetCentrePos();
        m_AOEObject.GetComponent<DebuffAOE>().m_AbilitySource = this;

        m_Snow.transform.SetParent(Player.m_Instance.gameObject.transform, false);
        m_Snow.transform.position = Player.m_Instance.GetCentrePos();

        m_Snow.SetActive(true);
        m_Fog.SetActive(true);
    }

    public override void UpdateTotalStats()
    {
        base.UpdateTotalStats();
        if (m_AOEObject == null) return;

        m_AOEObject.transform.localScale = new Vector2(m_TotalStats.AOE, m_TotalStats.AOE);
        ParticleSystem.MainModule fogMain = m_Fog.GetComponent<ParticleSystem>().main;
        fogMain.startLifetimeMultiplier = m_TotalStats.AOE / 2.2f;
        ParticleSystem.MainModule snowMain = m_Snow.GetComponent<ParticleSystem>().main;
        snowMain.startLifetimeMultiplier = m_TotalStats.AOE / 3f;
    }
}
