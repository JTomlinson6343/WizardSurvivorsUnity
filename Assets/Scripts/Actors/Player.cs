using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct PlayerStats
{
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator +(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed + right.speed;
        newstats.maxHealth = left.maxHealth + right.maxHealth;
        newstats.pullDist = left.pullDist + right.pullDist;
        newstats.iFramesMod = left.iFramesMod + right.iFramesMod;
        newstats.armor = left.armor + right.armor;
        return newstats;
    }
    // Overload + operator to allow two PlayerStats structs together
    public static PlayerStats operator -(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed - right.speed;
        newstats.maxHealth = left.maxHealth - right.maxHealth;
        newstats.pullDist = left.pullDist - right.pullDist;
        newstats.iFramesMod = left.iFramesMod - right.iFramesMod;
        newstats.armor = left.armor - right.armor;
        return newstats;
    }
    public static PlayerStats operator *(PlayerStats left, PlayerStats right)
    {
        PlayerStats newstats;
        newstats.speed = left.speed * right.speed;
        newstats.maxHealth = left.maxHealth * right.maxHealth;
        newstats.pullDist = left.pullDist * right.pullDist;
        newstats.iFramesMod = left.iFramesMod * right.iFramesMod;
        newstats.armor = left.armor * right.armor;
        return newstats;
    }
    public float speed;
    public float maxHealth;
    public float pullDist;
    public float iFramesMod;
    public float armor;

    public static PlayerStats Zero = new PlayerStats();
}

public class Player : Actor
{
    private readonly float m_kKnockback = 0.25f;

    [SerializeField] GameObject m_StaffPos;
    [SerializeField] GameObject m_CentrePos;
    [SerializeField] GameObject m_HealthBar;

    public static Player m_Instance;
    [SerializeField] PlayerStats m_BaseStats;
    PlayerStats m_BonusStats;
    PlayerStats m_TotalStats;

    public Ability m_ActiveAbility;
    public static bool m_AutoFire = false;

    float m_LastShot = 0;

    public float m_IFramesTime;
    public bool m_IsInvincible;

    [SerializeField] float m_HealSpeed;

    Vector3 staffStartPos;

    private Rigidbody2D m_RigidBody;
    private float m_Acceleration = 0.5f;

    [SerializeField] private Animator m_Animator;

    [SerializeField] private Animator m_AnimatorMask;
    [SerializeField] private SpriteRenderer spriteMask;

    private void Awake()
    {
        m_Instance = this;
        m_RigidBody = GetComponent<Rigidbody2D>();
        staffStartPos = m_StaffPos.transform.localPosition;
    }

    override public void Start()
    {
        base.Start();
        UpdateStats();
        ToggleAutoFire();
    }

    public override void Update()
    {
        Cursor.visible = Gamepad.current == null;
        if (StateManager.IsGameplayStopped()) { return; }

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Heal(25f);
        //}

        base.Update();

        MovementRoutine();

        HandleShootInput();

        UpdateStats();
    }

    private void HandleShootInput()
    {
        if (m_AutoFire) return;

        if (Input.GetMouseButton(0))
        {
            ShootMouse();
        }
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
        {
            ShootJoystick();
        }
    }

    public void ToggleAutoFire()
    {
        m_ActiveAbility.ToggleAutofire(m_AutoFire);
    }

    private void ShootMouse()
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastShot > m_ActiveAbility.GetTotalStats().cooldown)
        {
            m_ActiveAbility.OnMouseInput(GetMouseAimDirection().normalized);
            m_LastShot = now;
        }
    }
    private void ShootJoystick()
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastShot > m_ActiveAbility.GetTotalStats().cooldown)
        {
            m_ActiveAbility.OnMouseInput(GetControllerAimDirection());
            m_LastShot = now;
        }
    }

    private void MovementRoutine()
    {
        Vector3 currentVelocity = m_RigidBody.velocity;

        //Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 moveDir = Joystick.m_Instance.Direction;

        if (moveDir.x == 0 && moveDir.y == 0)
        {
            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (moveDir.magnitude > 1f) moveDir.Normalize();
        }

        moveDir.Normalize();

        Vector3 targetVelocity = moveDir * GetComponent<Player>().GetStats().speed;

        //currentVelocity = targetVelocity;
        currentVelocity += (targetVelocity - currentVelocity) * m_Acceleration;

        m_RigidBody.velocity = currentVelocity;

        SetAnimState(targetVelocity);
    }

    void SetAnimState(Vector3 targetVelocity)
    {
        FacingRoutine(targetVelocity);

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

    private void FacingRoutine(Vector3 targetVelocity)
    {
        FaceDirection(targetVelocity);

        if (m_AutoFire)
        {
            GameObject closestEnemy = Utils.GetClosestEnemyInRange(GetCentrePos(), m_ActiveAbility.m_DefaultAutofireRange);
            if (closestEnemy)
            {
                Vector2 dir = Utils.GetDirectionToGameObject(GetStaffTransform().position, closestEnemy);

                if (Mathf.Abs(dir.x) > 0.05f) FaceDirection(dir);
            }
            return;
        }

        if (Mathf.Abs(GetControllerAimDirection().x) > 0.1f)
        {
            FaceDirection(GetControllerAimDirection());
        }
        else if (Input.GetMouseButton(0) && Mathf.Abs(GetMouseAimDirection().x) > 0.05f)
        {
            FaceDirection(GetMouseAimDirection());
        }
    }

    private void FaceDirection(Vector2 dir)
    {
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();

        // If velocity > 0, don't flip. if it is less than, flip
        float faceDir = dir.x > 0 ? 1f : -1f;

        sprite.transform.localScale = new Vector2(Mathf.Abs(sprite.transform.localScale.x) * faceDir, sprite.transform.localScale.y);
    }

    // If actor has i-frames, return false. Else, return true
    override public DamageOutput TakeDamage(float amount)
    {
        if (m_IsInvincible)
        {
            return DamageOutput.invalidHit;
        }

        PlayerManager.m_Instance.StartShake(0.15f, 0.25f);

        amount -= m_TotalStats.armor;
        return OnDamage(amount);
    }

    protected override void StartFlashing()
    {
        m_IsInvincible = true;
        GetComponentInChildren<SpriteRenderer>().material = m_WhiteFlashMaterial;
        Invoke(nameof(EndFlashing), m_IFramesTime * (1f + m_TotalStats.iFramesMod));
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
        Destroy(m_HealthBar);
    }
    #region Stats Functions
    public void UpdateStats()
    {
        m_TotalStats = m_BaseStats + m_BonusStats;
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

    public Vector2 GetMouseAimDirection()
    {
        return (PlayerManager.m_Instance.m_Camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) - GetStaffTransform().position).normalized;
    }

    public Vector2 GetControllerAimDirection()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).normalized;
    }

    public PlayerStats GetStats()
    {
        return m_TotalStats;
    }

    public void IncreaseMaxHealth(float amount)
    {
        m_MaxHealth += amount;
        if (m_Health > m_MaxHealth) m_Health = m_MaxHealth;
        else m_Health += amount;
    }
    public void Heal(float amount)
    {
        StartCoroutine(HealAnim(amount,m_HealSpeed));
    }

    public void PercentHeal(float percent)
    {
        Heal(percent * m_MaxHealth);
    }

    private IEnumerator HealAnim(float amount, float healSpeed)
    {
        for (int i = 0; i < Mathf.RoundToInt(amount/healSpeed); i++)
        {
            m_Health += healSpeed;
            new WaitForEndOfFrame();
            yield return null;
        }
        yield return null;
    }

    public Transform GetStaffTransform()
    {
        return m_StaffPos.transform;
    }
    
    public Vector3 GetCentrePos()
    {
        return m_CentrePos.transform.position;
    }
    #endregion
}