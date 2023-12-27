using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LightningBolt : AOEObject
{
    float m_LengthModifier;
    float m_Lifetime;

    static readonly int kJumpLimit = 3;
    static int m_JumpCount = 0;

    [SerializeField] GameObject m_LightningPrefab;

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        transform.position = pos;// Player.m_Instance.GetStaffTransform().position;
        InitLengthModifier();
        m_Lifetime = lifetime;
        StartLifetimeTimer(lifetime);
        Zap();
    }

    private void InitLengthModifier()
    {
        m_LengthModifier = 1f / (transform.localScale.y * 0.9f);
    }

    public void Zap()
    {
        float range = Lightning.kBaseRange * m_AbilitySource.GetTotalStats().AOE;

        // Get position of furthest enemy in range
        Vector2 enemyPos = (Vector2)GameplayManager.GetFurthestEnemyInRange(transform.position, range).transform.position;

        GameplayManager.PointTowards(enemyPos, gameObject);

        // Extend lightning bolt towards enemy position
        GetComponent<SpriteRenderer>().size = new Vector2(
            GetComponent<SpriteRenderer>().size.x,
            Vector2.Distance((Vector2)transform.position, enemyPos) * m_LengthModifier
            );
    }

    protected override void OnEnemyHit(GameObject enemy)
    {
        if (m_AbilitySource.DamageOnCooldownCheck(enemy)) return;

        m_AbilitySource.StartDamageCooldown(enemy);

        base.OnEnemyHit(enemy);
        LightningJump(enemy);
    }

    // Function for when the lightning jumps to another target.
    private void LightningJump(GameObject enemy)
    {
        // If the lightning has already jumped the max number of times, return
        if (m_JumpCount >= kJumpLimit + m_AbilitySource.GetTotalStats().pierceAmount) return;
        // If there are no other enemies in range, return
        if (GameplayManager.GetAllEnemiesInRange(transform.position, Lightning.kBaseRange * m_AbilitySource.GetTotalStats().AOE).Count <= 1) return;

        GameObject newLightning = Instantiate(m_LightningPrefab);
        newLightning.GetComponent<LightningBolt>().Init(enemy.transform.position, m_AbilitySource, m_Lifetime);
        m_JumpCount++;
    }

    protected override void DestroySelf()
    {
        m_JumpCount--;
        base.DestroySelf();
    }
}
