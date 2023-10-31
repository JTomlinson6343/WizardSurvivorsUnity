using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : Ability
{
    GameObject m_AOEObject;
    [SerializeField] GameObject m_Particles;
    public override void OnCast()
    {
        base.OnCast();
        m_AOEObject = ProjectileManager.m_Instance.SpawnBlizzard(this, m_TotalStats.AOE);
        m_Particles.transform.SetParent(Player.m_Instance.gameObject.transform, false);
        m_Particles.transform.position = Player.m_Instance.GetCentrePos();
        ParticleSystem[] particles = m_Particles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.MainModule mainModule = particle.main;
            mainModule.startLifetimeMultiplier = m_TotalStats.AOE / 2.2f;
            particle.gameObject.SetActive(true);
        }
    }
}
