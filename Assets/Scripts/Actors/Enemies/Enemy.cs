using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Actor
{
    public Vector2 m_MoveDir;
    public bool m_FollowPlayer;

    public float m_HealthModifier;
    public float m_SpawnProbability; // The ratio of how common this spawns compared to other enemies
    public float m_MinWave; // Minimum wave number that enemy can spawn on

    [SerializeField] protected int m_XPAwarded;

    private bool m_IsChampion = false;
    private readonly float kChampSizeMod = 2f;
    private readonly int kChampXPMod = 2;
    private readonly float kChampHealthMod = 6f;
    private readonly Color kChampColour = new Color(0.8f, 0.8f, 0.8f);

    public float m_Speed;
    [SerializeField] protected float m_ContactDamage;
    [SerializeField] float m_KnockbackModifier;

    private readonly float m_kKnockback = 0.25f;
    private readonly float m_kBaseMoveSpeed = 2.0f;

    protected readonly float kSkillPointDropChance = 0.005f;
    private readonly int     m_MinChampSkillPoints = 2;
    private readonly int     m_MaxChampSkillPoints = 4;

    protected Rigidbody2D rb;
    protected Animator m_Animator;
    protected Animator m_AnimatorMask;

    [SerializeField] GameObject m_HealthbarPrefab;

    [SerializeField] GameObject m_DeathParticlesPrefab;

    virtual protected void Awake()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        m_Animator = animators[0];
        if (animators.Length>1) m_AnimatorMask = GetComponentsInChildren<Animator>()[1];
        rb = GetComponent<Rigidbody2D>();
    }

    override public void Start()
    {
        base.Start();
        m_Animator?.SetBool("Moving", true);
        m_AnimatorMask?.SetBool("Moving", true);

        m_FollowPlayer = true;
    }

    // Update is called once per frame
    override public void Update()
    {
        if (StateManager.IsGameplayStopped())
        {
            rb.velocity = Vector3.zero;
            return;
        }
        FollowPlayerCheck();
        RespawnCheck();
    }

    protected void FollowPlayerCheck()
    {
        if (m_isKnockedBack) return;
        
        if (m_FollowPlayer)
        {
            rb.velocity = m_kBaseMoveSpeed * m_Speed * m_MoveDir;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    protected void RespawnCheck()
    {
        if (!Player.m_Instance) return;

        if (Vector2.Distance(Player.m_Instance.transform.position, transform.position) < EnemyManager.m_Instance.m_SpawnRadius) return;

        EnemyManager.m_Instance.OnRespawn();
        DestroyEnemy();
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (m_ContactDamage <= 0f) return;
        GameObject otherObject = collision.gameObject;

        if (m_Stunned) return;
        if (!otherObject.GetComponent<Player>()) return;
        if (otherObject.GetComponent<Player>().m_IsInvincible) return;

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

    public void SetAnimState(Vector3 dir)
    {
        FaceForward(dir);
    }

    virtual protected void FaceForward(Vector3 targetVelocity)
    {
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();

        // If velocity > 0, don't flip. if it is less than, flip
        float faceDir = targetVelocity.x > 0 ? 1f : -1f;

        sprite.transform.localScale = new Vector2(Mathf.Abs(sprite.transform.localScale.x) * faceDir, sprite.transform.localScale.y);
    }

    private void RollForSkillPoint()
    {
        if (Random.Range(0f, 1f) > kSkillPointDropChance) return;

        ProgressionManager.m_Instance.SpawnSkillPoint(transform.position, 1);
    }

    protected override void OnDeath()
    {
        DestroyEnemy();

        if (m_IsChampion) ChampionDeath();
        else              NormalDeath();

        ProgressionManager.m_Instance.SpawnXP(transform.position, m_XPAwarded);
        ProgressionManager.m_Instance.IncrementEnemyKills();
        EnemyManager.m_Instance.IncrementEnemiesKilled();
    }

    public void DestroyEnemy()
    {
        EnemyManager.m_Enemies.Remove(this);
        Destroy(gameObject);
    }

    virtual protected void NormalDeath()
    {
        RollForSkillPoint();
        DeathParticlesRoutine();
    }

    protected void DeathParticlesRoutine()
    {
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
        AddHealthBar();
    }

    public void AddHealthBar()
    {
        GameObject hpBar = Instantiate(m_HealthbarPrefab);
        hpBar.transform.SetParent(transform, false);
        hpBar.GetComponentInChildren<BasicBar>().m_Actor = this;
    }
}