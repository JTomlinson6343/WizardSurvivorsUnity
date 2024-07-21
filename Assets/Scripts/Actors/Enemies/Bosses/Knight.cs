using System.Collections;
using Unity.VisualScripting;
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

    [SerializeField] Transform m_ShadowTransform;

    [SerializeField] float m_MeleeRadius;
    [SerializeField] Transform m_Sword;
    [SerializeField] float m_SwordRadius;
    [SerializeField] float m_SwordDamage;

    float m_NoSwordHitsTimer = 0; // Time without damaging the player with the sword
    [SerializeField] float m_NoSwordHitsMaxTime; // Max time allowed in running state without hitting the player

    float m_RunningStateTimer = 0; // Amount of time that has been spent in running state
    [SerializeField] float m_RunningStateMaxTime; // Max time before changing state out of running state

    [SerializeField] float m_JumpDelay;
    [SerializeField] float m_JumpSpeed;
    [SerializeField] float m_JumpDuration;
    [SerializeField] float m_MidAirDuration;

    [SerializeField] GameObject m_SpearPrefab;

    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileDamage;
    [SerializeField] float m_ProjectileKnockback;
    [SerializeField] float m_ProjectileCooldown;
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

    public override void BossFightInit()
    {
        PlayerManager.m_Instance.m_ActorsToBind = new Actor[] { Player.m_Instance };
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
                m_RunningStateTimer += Time.deltaTime;
                m_NoSwordHitsTimer += Time.deltaTime;
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
                m_Animator.Play("Idle");
                rb.velocity = Vector2.zero;
                if (m_NoSwordHitsTimer < m_NoSwordHitsMaxTime && m_RunningStateTimer < m_RunningStateMaxTime) SetState(State.Running);
                else
                {
                    StartCoroutine(Utils.DelayedCall(m_JumpDelay, () =>
                    {
                        SetState(State.Jumping);
                    }));
                }
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
                break;
            case State.Jumping:
                m_Animator.Play("Jump");
                ToggleDamageImmune(true);
                m_RunningStateTimer = 0;
                m_NoSwordHitsTimer = 0;
                StartCoroutine(MidAir());
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
            m_NoSwordHitsTimer = 0;
            DamageInstanceData data = new(gameObject, player.gameObject);
            data.amount = m_SwordDamage;
            data.damageType = DamageType.Physical;
            data.doDamageNumbers = true;

            DamageManager.m_Instance.DamageInstance(data, player.transform.position);
        }
    }

    IEnumerator MidAir()
    {
        PlayerBounds bounds = PlayerManager.m_Instance.m_BossArenaBounds;

        Transform animTransform = m_Animator.transform;

        Vector2 initShadowScale = m_ShadowTransform.localScale;

        Vector3 upMovement = m_JumpSpeed * Time.deltaTime * new Vector2(0, 1f);
        float shadowShrinkAmount = 1f - 2f * Time.deltaTime;

        while (bounds.IsInBounds(animTransform.position, 1.5f))
        {
            animTransform.position += upMovement;
            m_ShadowTransform.localScale = new Vector2(m_ShadowTransform.localScale.x * shadowShrinkAmount, m_ShadowTransform.localScale.y * shadowShrinkAmount);

            yield return new WaitForEndOfFrame();
        }

        m_ShadowTransform.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        SetState(State.InAir);

        float elapsed = 0f;

        while (elapsed < m_MidAirDuration)
        {
            Vector2 startPos = new Vector2(Random.Range(bounds.left, bounds.right), bounds.top + 0.5f);
            ProjectileManager.m_Instance.EnemyShot(
                startPos,
                Utils.GetDirectionToGameObject(startPos, Player.m_Instance.gameObject),
                m_ProjectileSpeed,
                m_ProjectileLifetime,
                m_SpearPrefab,
                m_ProjectileDamage,
                m_ProjectileKnockback,
                gameObject,
                DamageType.Physical);

            elapsed += m_ProjectileCooldown;
            yield return new WaitForSeconds(m_ProjectileCooldown);
        }

        Vector2 landPos = Player.m_Instance.transform.position + new Vector3(3f, 0f);

        transform.position = new Vector2(landPos.x, transform.position.y);

        m_ShadowTransform.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        SetState(State.Falling);

        while (animTransform.localPosition.y > 0f)
        {
            animTransform.position -= upMovement;
            m_ShadowTransform.localScale = new Vector2(m_ShadowTransform.localScale.x / shadowShrinkAmount, m_ShadowTransform.localScale.y / shadowShrinkAmount);

            yield return new WaitForEndOfFrame();
        }

        SetState(State.Idle);
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
        m_Targetable = !on;
    }
}
