using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Ability
{
    GameObject m_AOEObject;
    [SerializeField] GameObject m_Fog;
    [SerializeField] GameObject m_Snow;
    public override void OnCast()
    {
        base.OnCast();
        m_AOEObject = ProjectileManager.m_Instance.SpawnBlizzard(this, m_TotalStats.AOE);
        m_Snow.transform.SetParent(Player.m_Instance.gameObject.transform, false);
        m_Snow.transform.position = Player.m_Instance.GetCentrePos();

        ParticleSystem.MainModule fogMain = m_Fog.GetComponent<ParticleSystem>().main;
        fogMain.startLifetimeMultiplier = m_TotalStats.AOE / 2.2f;
        ParticleSystem.MainModule snowMain = m_Snow.GetComponent<ParticleSystem>().main;
        snowMain.startLifetimeMultiplier = m_TotalStats.AOE / 3f;

        m_Snow.SetActive(true);
        m_Fog.SetActive(true);
    }
}
