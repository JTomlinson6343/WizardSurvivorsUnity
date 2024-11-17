using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AOEObject : Projectile
{
    [SerializeField] float m_ActiveFramesDuration;

    readonly float kLifetimeToAOERatio = 4.4f;

    float m_TimeSpawned;

    private void Awake()
    {
        m_TimeSpawned = Time.realtimeSinceStartup;
    }

    protected override void OnTargetHit(GameObject enemy)
    {
        if (m_ActiveFramesDuration > 0f && Time.realtimeSinceStartup - m_TimeSpawned > m_ActiveFramesDuration) return;

        if (m_HitTargets.Contains(enemy)) return;
        m_HitTargets.Add(enemy);

        ProjectileManager.m_Instance.EndTargetCooldown(enemy, m_HitboxDelay, m_HitTargets);

        enemy.GetComponent<Actor>().KnockbackRoutine(Utils.GetDirectionToGameObject(transform.position, enemy), m_AbilitySource.GetTotalStats().knockback);

        DamageTarget(enemy);
    }

    protected override void DamageTarget(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, enemy);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = m_AbilitySource.m_Data.damageType;
        data.target = enemy;
        data.abilitySource = m_AbilitySource;
        DamageManager.m_Instance.DamageInstance(data, enemy.transform.position);
    }

    public virtual void Init(Vector2 pos, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        transform.localScale = new Vector2(ability.GetTotalStats().AOE, ability.GetTotalStats().AOE);
        StartLifetimeTimer(lifetime);
        transform.position = pos;
    }

    public void SetParticles()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        if (particles.Length <= 0) return;

        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.startLifetimeMultiplier = m_AbilitySource.GetTotalStats().AOE / kLifetimeToAOERatio;
        }
    }

    public void Init(GameObject parent, Vector2 pos, Ability ability, float lifetime)
    {
        transform.SetParent(parent.transform);
        Init(pos, ability, lifetime);
    }
}