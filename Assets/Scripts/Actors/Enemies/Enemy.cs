using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public float m_HealthModifier;
    public float m_SpawnProbability; // The ratio of how common this spawns compared to other enemies

    [SerializeField] protected int m_XPAwarded;
    [SerializeField] float m_SkillPointDropChance;

    [SerializeField] int m_MinChampSkillPoints;
    [SerializeField] int m_MaxChampSkillPoints;
    private bool m_IsChampion = false;
    private readonly float kChampSizeMod = 2f;
    private readonly int kChampXPMod = 2;
    private readonly float kChampHealthMod = 3f;
    private readonly Color kChampColour = new Color(1, 0.3f, 0);

    [SerializeField] float m_Speed;
    [SerializeField] float m_ContactDamage;
    [SerializeField] float m_KnockbackModifier;

    private readonly float m_kKnockback = 0.25f;
    private readonly float m_kBaseMoveSpeed = 2.2f;

    private Rigidbody2D rb;
    private Animator m_Animator;

    [SerializeField] GameObject m_HealthbarPrefab;

    [SerializeField] GameObject m_DeathParticlesPrefab;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        if (StateManager.IsGameplayStopped())
        {
            rb.velocity = Vector3.zero;
            return;
        }
        RespawnCheck();
        FollowPlayer();
    }

    protected void FollowPlayer()
    {
        Vector3 currentPos = gameObject.transform.position;
        if (Player.m_Instance != null)
        {
            Vector3 playerPos = Player.m_Instance.transform.position;
            Vector3 moveDir = (playerPos - currentPos).normalized;
            Vector3 targetVelocity = moveDir * m_kBaseMoveSpeed * m_Speed;
            Vector3 currentVelocity = rb.velocity;

            currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * 5.0f;

            rb.velocity = currentVelocity;

            SetAnimState(targetVelocity);
        }
    }

    private void RespawnCheck()
    {
        if (!Player.m_Instance) return;

        if (Vector2.Distance(Player.m_Instance.transform.position, transform.position) < EnemyManager.m_Instance.m_SpawnRadius) return;

        EnemyManager.m_Instance.OnRespawn();
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        Vector2 objectPos = otherObject.transform.position;
        Vector2 currentPos = gameObject.transform.position;

        Vector2 moveDir = (objectPos - currentPos).normalized;

        //Vector3 currentVelocity = m_RigidBody.velocity;

        //currentVelocity -= moveDir * 1.0f;

        if (otherObject.GetComponent<Player>() && !otherObject.GetComponent<Player>().m_IsInvincible)
        {
            Enemy enemy = GetComponent<Enemy>();
            DamageInstanceData data = new(gameObject, otherObject);
            data.amount = enemy.m_ContactDamage;
            data.damageType = DamageType.Physical;
            data.doDamageNumbers = true;
            Rigidbody2D playerBody = otherObject.GetComponent<Rigidbody2D>();

            DamageManager.m_Instance.DamageInstance(data, transform.position);

            // Knock player back
            playerBody.AddForce(GetComponent<Rigidbody2D>().velocity.normalized * m_kKnockback * m_KnockbackModifier);
        }
    }

    void SetAnimState(Vector3 targetVelocity)
    {
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();

        // If velocity > 0, don't flip. if it is less than, flip
        sprite.flipX = targetVelocity.x > 0 ? false : true;

        if (targetVelocity.magnitude > 0)
        {
            // Object is moving
            m_Animator.SetBool("Moving", true);
            m_Animator.SetBool("Idle", false); // Disable the idle state
        }
        else
        {
            // Object is not moving
            m_Animator.SetBool("Moving", false); // Disable the moving state
            m_Animator.SetBool("Idle", true);
        }
    }

    private void RollForSkillPoint()
    {
        if (Random.Range(0f, 1f) > m_SkillPointDropChance) return;

        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, 1);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        if (m_IsChampion) ChampionDeath();
        else              NormalDeath();

        ProgressionManager.m_Instance.SpawnXP(transform.position, m_XPAwarded);
        ProgressionManager.m_Instance.AddScore(m_XPAwarded);
        ProgressionManager.m_Instance.IncrementEnemyKills();
        EnemyManager.m_Instance.IncrementEnemiesKilled();
    }

    private void NormalDeath()
    {
        RollForSkillPoint();
        if (!m_DeathParticlesPrefab) return;

        GameObject smoke = Instantiate(m_DeathParticlesPrefab);
        smoke.transform.position = transform.position;
    }

    private void ChampionDeath()
    {
        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, Random.Range(m_MinChampSkillPoints, m_MaxChampSkillPoints+1));
        ProgressionManager.m_Instance.IncrementChampionKills();

        if (!m_DeathParticlesPrefab) return;

        GameObject smoke = Instantiate(m_DeathParticlesPrefab);
        smoke.transform.localScale *= kChampSizeMod;
        smoke.transform.position = transform.position;
    }

    public void MakeChampion()
    {
        m_IsChampion = true;
        transform.localScale *= kChampSizeMod;
        m_MaxHealth *= kChampHealthMod;
        m_XPAwarded *= kChampXPMod;
        GetComponentInChildren<SpriteRenderer>().color = kChampColour;
        GameObject hpBar = Instantiate(m_HealthbarPrefab);
        hpBar.transform.SetParent(transform, false);
        hpBar.GetComponentInChildren<BasicBar>().m_Actor = this;
    }
}