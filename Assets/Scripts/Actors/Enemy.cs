using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public float m_HealthModifier;
    public float m_SpawnProbability; // The ratio of how common this spawns compared to other enemies

    [SerializeField] int m_XPAwarded;
    [SerializeField] float m_SkillPointDropChance;
    [SerializeField] float m_Speed;
    [SerializeField] float m_ContactDamage;
    [SerializeField] float m_KnockbackModifier;

    private readonly float m_kKnockback = 0.25f;
    private readonly float m_kBaseMoveSpeed = 2.3f;

    private Rigidbody2D m_RigidBody;
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING)
        {
            m_RigidBody.velocity = Vector3.zero;
            return;
        }

        Vector3 currentPos = gameObject.transform.position;
        if (Player.m_Instance != null)
        {
            Vector3 playerPos = Player.m_Instance.transform.position;
            Vector3 moveDir = (playerPos - currentPos).normalized;
            Vector3 targetVelocity = moveDir * m_kBaseMoveSpeed * m_Speed;
            Vector3 currentVelocity = m_RigidBody.velocity;

            currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * 5.0f;

            m_RigidBody.velocity = currentVelocity;

            SetAnimState(targetVelocity);
        }
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
            data.doDamageNumbers = false;
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

        ProgressionManager.m_Instance.SpawnXP(transform.position, m_XPAwarded);
        RollForSkillPoint();
        ProgressionManager.m_Instance.AddScore(m_XPAwarded);
        ProgressionManager.m_Instance.IncrementEnemyKills();
        EnemySpawner.m_Instance.IncrementEnemiesKilled();
    }

    public void MakeChampion()
    {
        transform.localScale *= 2f;
        m_MaxHealth *= 2f;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}