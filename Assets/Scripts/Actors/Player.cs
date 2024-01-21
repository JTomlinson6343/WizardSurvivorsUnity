using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PlayerStats
{
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator +(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed + right.speed;
        newstats.shotSpeed = left.shotSpeed + right.shotSpeed;
        newstats.maxHealth = left.maxHealth + right.maxHealth;
        newstats.healthRegen = left.healthRegen + right.healthRegen;
        return newstats;
    }
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator -(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed - right.speed;
        newstats.shotSpeed = left.shotSpeed - right.shotSpeed;
        newstats.maxHealth = left.maxHealth - right.maxHealth;
        newstats.healthRegen = left.healthRegen - right.healthRegen;
        return newstats;
    }

    public static PlayerStats operator *(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed * right.speed;
        newstats.shotSpeed = left.shotSpeed * right.shotSpeed;
        newstats.maxHealth = left.maxHealth * right.maxHealth;
        newstats.healthRegen = left.healthRegen * right.healthRegen;
        return newstats;
    }
    public float speed;
    public float shotSpeed;
    public float maxHealth;
    public float healthRegen;

    public static PlayerStats Zero = new PlayerStats();
}

public class Player : Actor
{
    private readonly float m_kKnockback = 0.25f;

    [SerializeField] GameObject staffPos;
    [SerializeField] GameObject centrePos;

    public static Player m_Instance;
    [SerializeField] PlayerStats m_BaseStats;
    PlayerStats m_BonusStats;
    PlayerStats m_TotalStats;

    public Ability m_ActiveAbility;

    private float m_LastHit = 0.0f;
    float m_LastShot = 0;

    public float m_IFramesTime;
    public bool m_IsInvincible;

    Vector3 staffStartPos;

    private Rigidbody2D m_RigidBody;
    private float m_Acceleration = 50.0f;

    [SerializeField] private Animator m_Animator;

    [SerializeField] private Animator m_AnimatorMask;
    [SerializeField] private SpriteRenderer spriteMask;

    private void Awake()
    {
        m_Instance = this;
        m_RigidBody = GetComponent<Rigidbody2D>();
        staffStartPos = staffPos.transform.localPosition;
    }

    override public void Start()
    {
        base.Start();
        UpdateStats();
    }

    public override void Update()
    {
        if (StateManager.GetCurrentState() == State.PAUSED) { return; }

        base.Update();

        MovementRoutine();

        if (Input.GetMouseButton(0))
        {
            ShootMouse();
        }
        //if (Input.GetAxis("4") > 0.25f || Input.GetAxis("5") > 0.25f)
        //{
        //    ShootJoystick();
        //}
        UpdateStats();
    }

    private void ShootMouse()
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastShot > m_ActiveAbility.GetTotalStats().cooldown)
        {
            m_ActiveAbility.OnMouseInput(GetAimDirection().normalized);
            m_LastShot = now;
        }
    }
    private void ShootJoystick()
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastShot > m_ActiveAbility.GetTotalStats().cooldown)
        {
            m_ActiveAbility.OnMouseInput(new Vector2(Input.GetAxis("5"), Input.GetAxis("4")).normalized);
            m_LastShot = now;
        }
    }

    private void MovementRoutine()
    {
        Vector3 currentVelocity = m_RigidBody.velocity;

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;

        Vector3 targetVelocity = moveDir * GetComponent<Player>().GetStats().speed;

        //currentVelocity = targetVelocity;
        currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * m_Acceleration;

        m_RigidBody.velocity = currentVelocity;

        SetAnimState(targetVelocity);
    }

    void SetAnimState(Vector3 targetVelocity)
    {
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();

        // If player is moving right, face right
        if (targetVelocity.x > 0)
        {
            sprite.flipX = false;
            spriteMask.flipX = false;
            staffPos.transform.localPosition = staffStartPos;
        }
        // If player is moving left, face left
        else if (targetVelocity.x < 0)
        {
            sprite.flipX = true;
            spriteMask.flipX = true;
            staffPos.transform.localPosition = staffStartPos * new Vector2(-1, 1);
        }

        if (targetVelocity.magnitude > 0)
        {
            // Object is moving
            m_Animator.SetBool("Moving", true);
            m_Animator.SetBool("Idle", false); // Disable the idle state

            m_AnimatorMask.SetBool("Moving", true);
            m_AnimatorMask.SetBool("Idle", false); // Disable the idle state
        }
        else
        {
            // Object is not moving
            m_Animator.SetBool("Moving", false); // Disable the moving state
            m_Animator.SetBool("Idle", true);

            m_AnimatorMask.SetBool("Moving", false);
            m_AnimatorMask.SetBool("Idle", true); // Disable the idle state
        }
    }

    // If actor has i-frames, return false. Else, return true
    override public DamageOutput TakeDamage(float amount)
    {
        if (m_IsInvincible)
        {
            return DamageOutput.invalidHit;
        }

        return OnDamage(amount);
    }

    protected override void StartFlashing()
    {
        m_IsInvincible = true;
        GetComponentInChildren<SpriteRenderer>().material = m_WhiteFlashMaterial;
        Invoke(nameof(EndFlashing), m_IFramesTime);
    }

    protected override void EndFlashing ()
    {
        m_IsInvincible = false;
        base.EndFlashing();
    }

    protected override void OnDeath()
    {
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        ProgressionManager.m_Instance.GameOver();
    }
    #region Stats Functions
    public void UpdateStats()
    {
        m_TotalStats = m_BaseStats + m_BonusStats;
        UpdateHealth();
    }
    public void AddBonusStats(PlayerStats stats)
    {
        m_BonusStats += stats;
    }

    public void AddTempStats(PlayerStats stats, float duration)
    {
        m_BonusStats += stats;
        //In Start() or wherever
        StartCoroutine(RemoveTempStats(stats, duration));
    }

    public override void KnockbackRoutine(Vector2 dir, float knockbackMagnitude)
    {
        knockbackMagnitude = Mathf.Clamp01(1f - m_KnockbackResist) * knockbackMagnitude;
        GetComponent<Rigidbody2D>().AddForce(dir.normalized * knockbackMagnitude * m_kKnockback);
    }

    private IEnumerator RemoveTempStats(PlayerStats stats, float duration)
    {
        yield return new WaitForSeconds(duration);

        m_BonusStats -= stats;
    }
    #endregion

    #region Getters
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector2 GetAimDirection()
    {
        return (PlayerManager.m_Instance.m_Camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - GetStaffTransform().position).normalized;
    }

    public PlayerStats GetStats()
    {
        return m_TotalStats;
    }
    public void UpdateHealth()
    {
        float ratio = GetHealthAsRatio();
        m_MaxHealth = m_TotalStats.maxHealth;
        m_Health = m_MaxHealth * ratio;
    }

    public Transform GetStaffTransform()
    {
        return staffPos.transform;
    }
    
    public Vector3 GetCentrePos()
    {
        return centrePos.transform.position;
    }
    #endregion
}