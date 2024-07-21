using UnityEngine;

public class Knight : Boss
{
    private enum State
    {
        Idle,
        Running,
        Attacking,
        Jumping,
        InAir,
        Falling,
    }

    State m_State;

    private float m_InitSpeed;

    [SerializeField] float m_MeleeRadius;
    [SerializeField] Transform m_Sword;
    [SerializeField] float m_SwordRadius;
    [SerializeField] float m_SwordDamage;

    float m_NoSwordHitsTime = 0; // Time without damaging the player with the sword
    [SerializeField] float m_NoSwordHitsMaxTimer; // Max time allowed in running state without hitting the player

    float m_RunningStateTimer = 0; // Amount of time that has been spent in running state
    [SerializeField] float m_RunningStateMaxTime; // Max time before changing state out of running state

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_Sword.position, m_SwordRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_MeleeRadius);
    }

    public override void Enraged(int bossNumber)
    {
        throw new System.NotImplementedException();
    }

    override protected void Awake()
    {
        base.Awake();

        m_InitSpeed = m_Speed;

        SetState(State.Running);
    }

    public override void Update()
    {
        if (Player.m_Instance == null) return;

        Brain();
    }

    private void Brain()
    {
        Vector2 dirToPlayer = Utils.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject);
        switch (m_State)
        {
            case State.Idle:
                FaceForward(dirToPlayer);
                break;
            case State.Running:
                FaceForward(dirToPlayer);
                m_NoSwordHitsTime += Time.deltaTime;
                if (Vector2.Distance(Player.m_Instance.transform.position, transform.position) < m_MeleeRadius) SetState(State.Attacking);
                else
                {
                    rb.velocity = dirToPlayer * m_Speed;
                }
                break;
            case State.Attacking:
                break;
            case State.InAir:
            case State.Jumping:
            case State.Falling:
                break;
            default:
                break;
        }
    }

    private void SetState(State state)
    {
        m_State = state;
        switch (state)
        {
            case State.Idle:
                ToggleDamageImmune(false);
                rb.velocity = Vector2.zero;
                if (m_NoSwordHitsTime >= m_NoSwordHitsMaxTimer || m_RunningStateTimer >= m_RunningStateMaxTime) SetState(State.Jumping);
                else SetState(State.Running);
                return;
            case State.Running:
                m_Animator.Play("Walk");
                return;
            case State.Attacking:
                PlayMethodAfterAnimation("Attack", 0.5f, () =>
                {
                    SetState(State.Idle);
                });
                StartCoroutine(Utils.DelayedCall(0.3f, () =>
                {
                    Attack();
                }));
                rb.velocity = Vector2.zero;
                return;
            case State.InAir:
            case State.Jumping:
                m_Animator.Play("Jump");
                m_RunningStateTimer = 0;
                m_NoSwordHitsMaxTimer = 0;
                ToggleDamageImmune(true);
                return;
            case State.Falling:
                m_Animator.Play("Fall");
                return;
        }
    }

    private void Attack()
    {
        Player player = Player.m_Instance;
        if (Vector2.Distance(player.transform.position, m_Sword.position) < m_SwordRadius)
        {
            DamageInstanceData data = new(gameObject, player.gameObject);
            data.amount = m_SwordDamage;
            data.damageType = DamageType.Physical;
            data.doDamageNumbers = true;

            DamageManager.m_Instance.DamageInstance(data, player.transform.position);
        }
    }

    private void Land()
    {
        m_State = State.Falling;
    }

    protected override void FaceForward(Vector3 targetVelocity)
    {
        // If velocity > 0, don't flip. if it is less than, flip
        float faceDir = targetVelocity.x > 0 ? 1f : -1f;

        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * faceDir, transform.localScale.y);
    }

    // Called whenever this actor is knocked back
    override public void KnockbackRoutine(Vector2 dir, float knockbackMagnitude)
    {
        m_Speed = m_InitSpeed * 0.3f;
        StartCoroutine(Utils.DelayedCall(0.2f, ()=>
        {
            m_Speed = m_InitSpeed;
        }));
    }

    private void ToggleDamageImmune(bool on)
    {
        GetComponentInChildren<Collider2D>().enabled = !on;
    }
}
