using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LightningBolt : AOEObject
{
    float m_LengthModifier;

    static readonly int kJumpLimit = 3;
    static int kJumpCount = 0;
    readonly float kBaseRange = 5f;

    [SerializeField] GameObject m_LightningPrefab;

    private void Start()
    {
        InitLengthModifier();
    }

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        StartLifetimeTimer(lifetime);
        transform.position = pos;
        InitLengthModifier();
        print(m_LengthModifier);
    }

    private void InitLengthModifier()
    {
        m_LengthModifier = 1f / (transform.localScale.y * 0.9f);
    }

    private void Update()
    {
        //Vector2 enemyPos = GameplayManager.GetGameObjectCentre(GameplayManager.GetClosestEnemy(transform.position).gameObject);

        Vector2 enemyPos = new Vector2(GameplayManager.GetClosestEnemyPos(transform.position).x, GameplayManager.GetClosestEnemyPos(transform.position).y+1f);

        if (Vector2.Distance(enemyPos, transform.position) > kBaseRange * m_AbilitySource.GetTotalStats().AOE)
        {
            GetComponent<SpriteRenderer>().size = new Vector2(
            GetComponent<SpriteRenderer>().size.x,
            0f
            );
            return;
        }

        if (enemyPos == Vector2.negativeInfinity) return;

        Vector2 dir = GameplayManager.GetDirectionToClosestEnemy(transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

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
        if (kJumpCount >= kJumpLimit) return;

        GameObject newLightning = Instantiate(m_LightningPrefab);
        newLightning.GetComponent<LightningBolt>().Init(enemy.transform.position, m_AbilitySource, 0.4f);
        kJumpCount++;
    }

    protected override void DestroySelf()
    {
        kJumpCount--;
        base.DestroySelf();
    }
}
