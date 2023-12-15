using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Reflection;

public class FireDebuff : Debuff
{
    public GameObject m_FireParticlePrefab;
    public Light2D m_LightPrefab;
    private GameObject m_FireEffect;
    private Light2D m_Light;

    public override void Init(float debuffTime, float damage, DamageType damageType, GameObject source, bool percentHealth, int maxStacks, DebuffType debuffType)
    {
        base.Init(debuffTime, damage, damageType, source, percentHealth, maxStacks, debuffType);

        if (GetComponentInChildren<ParticleSystem>() != null )
            return;

        m_FireEffect = Instantiate(m_FireParticlePrefab);
        //m_FireEffect.transform.SetParent(gameObject.transform, false);

        m_Light = gameObject.AddComponent<Light2D>();
        m_Light.intensity = 5;
        m_Light.color = m_LightPrefab.color;
        m_Light.pointLightInnerRadius = m_LightPrefab.pointLightInnerRadius;
        m_Light.pointLightOuterRadius = m_LightPrefab.pointLightOuterRadius;
        m_Light.pointLightOuterAngle = m_LightPrefab.pointLightOuterAngle;
        m_Light.pointLightInnerAngle = m_LightPrefab.pointLightInnerAngle;
        m_Light.falloffIntensity = m_LightPrefab.falloffIntensity;

        AudioManager.m_Instance.PlaySound(8);
    }

    protected override void EndDebuff()
    {
        Destroy(m_FireEffect);
        Destroy(m_Light);
        base.EndDebuff();
    }

    private void Update()
    {
        m_FireEffect.transform.position = transform.position;
    }

    private void OnDestroy()
    {
        Destroy(m_FireEffect);
    }
}
