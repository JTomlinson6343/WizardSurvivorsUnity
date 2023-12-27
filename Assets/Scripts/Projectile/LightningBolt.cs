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
    static int m_JumpCount = 0;

    [SerializeField] GameObject m_LightningPrefab;

    private void Start()
    {
        Init(new Vector2(0, 0), m_AbilitySource, 0.2f);
    }

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        transform.position = pos;
        InitLengthModifier();
        Zap();
    }

    private void InitLengthModifier()
    {
        m_LengthModifier = 1f / (transform.localScale.y * 0.9f);
    }

    private void ZeroLength()
    {
        GetComponent<SpriteRenderer>().size = new Vector2(
            GetComponent<SpriteRenderer>().size.x,
            0f
            );
    }

    public void Zap()
    {
        float range = Lightning.kBaseRange * m_AbilitySource.GetTotalStats().AOE;

        Vector2 enemyPos = (Vector2)GameplayManager.GetFurthestEnemyInRange(transform.position, range).transform.position;

        Vector2 dir = GameplayManager.GetDirectionToEnemy(
            transform.position, GameplayManager.GetFurthestEnemyInRange(transform.position,
            range));

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
        if (m_JumpCount >= kJumpLimit) return;

        GameObject newLightning = Instantiate(m_LightningPrefab);
        newLightning.GetComponent<LightningBolt>().Init(enemy.transform.position, m_AbilitySource, 0.4f);
        m_JumpCount++;
    }

    protected override void DestroySelf()
    {
        m_JumpCount--;
        base.DestroySelf();
    }
}
