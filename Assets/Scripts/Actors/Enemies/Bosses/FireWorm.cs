using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : Boss
{
    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] GameObject m_FireDropPrefab;

    [SerializeField] GameObject m_Mouth;
    private GameObject m_DirtParticles;
    [SerializeField] CapsuleCollider2D m_NormalCollider;
    [SerializeField] CircleCollider2D m_BurrowedCollider;

    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileDamage;
    [SerializeField] float m_ProjectileKnockback;
    [SerializeField] float m_ProjectileCooldown;

    [SerializeField] float m_FireDropDamage;

    [SerializeField] bool m_Burrowed;
    [SerializeField] float m_BurrowChance;
    [SerializeField] float m_BurrowCooldown;
    [SerializeField] float m_BurrowedSpeed;
    [SerializeField] float m_DropFireCooldown;
    [SerializeField] float m_MinBurrowDuration;
    [SerializeField] float m_MaxBurrowDuration;
    [SerializeField] float m_ChargeDelay;
    [SerializeField] float m_ChargeDuration;
    bool m_Charging;

    private bool m_ProjectileOnCooldown;
    private bool m_BurrowOnCooldown;

    public override void Enraged(int bossNumber)
    {
        m_MaxHealth *= bossNumber;
        m_ProjectileCooldown *= 1f / bossNumber;
        m_BurrowedSpeed *= 1f + bossNumber * 0.05f;
        m_MinBurrowDuration *= 1f + bossNumber * 0.15f;
        m_MaxBurrowDuration *= 1f + bossNumber * 0.15f;
        m_ChargeDelay *= 1f - bossNumber * 0.1f;

        m_BossName = "Enraged " + m_BossName;
        GetComponentInChildren<SpriteRenderer>().color = Color.gray;
    }

    public override void Start()
    {
        base.Start();
        m_DirtParticles = GetComponentInChildren<ParticleSystem>().gameObject;

        ToggleBurrow(false);
        m_BurrowOnCooldown = true;

        StartCoroutine(Utils.DelayedCall(3f, () =>
        {
            m_BurrowOnCooldown = false;
        }));
    }

    public override void Update()
    {
        if (StateManager.GetCurrentState() != StateManager.State.BOSS) return;

        Brain();
    }

    private void Brain()
    {
        if (Player.m_Instance == null) return;
        if (m_IsMidAnimation) return;

        if (!m_Burrowed)
        {
            if (!m_BurrowOnCooldown) BurrowCheck();
            
            rb.velocity = Vector2.zero;
            if (!m_ProjectileOnCooldown) StartCoroutine(Shoot());
            else
            {
                GetComponentInChildren<Animator>().Play("Idle");
                FaceForward(Player.m_Instance.transform.position - transform.position);
            }
        }
        else
        {
            if (!PlayerManager.m_Instance.m_BossArenaBounds.IsInBounds(transform.position, 0.2f)) rb.velocity = rb.velocity;
        }
    }

    private void BurrowCheck() // Try and burrow if the rng value is correct
    {
        if (Random.Range(0f, 1f) <= m_BurrowChance)
        {
            PlayMethodAfterAnimation("Burrow", 0.15f, () => {
                Animator animator = GetComponentInChildren<Animator>();

                animator.Play("Idle", -1, 0f);
                ToggleBurrow(true);
                StartCoroutine(Burrow());
            });
        }
    }

    private IEnumerator Burrow()
    {
        float duration = Random.Range(m_MinBurrowDuration, m_MaxBurrowDuration);
        float timeLeft = duration;

        // Charge at the player after a delay
        yield return Charge();

        while (timeLeft > 0f)
        {
            if (!m_Charging)
            {

                yield return Charge();
            }

            timeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ToggleBurrow(false);
        rb.velocity = Vector2.zero;
        m_BurrowOnCooldown = true;

        yield return new WaitForSeconds(m_BurrowCooldown);

        m_BurrowOnCooldown = false;
    }

    private IEnumerator DropFire()
    {
        while (m_Burrowed)
        {
            if (m_Charging)
            {
                SpawnFireDrop();
            }
            yield return new WaitForSeconds(m_DropFireCooldown);
        }
    }

    void SpawnFireDrop()
    {
        GameObject fire = Instantiate(m_FireDropPrefab);
        fire.GetComponent<EnemyAOE>().Init(transform.position, m_FireDropDamage, 0, 5, gameObject, DamageType.Fire);
    }

    private IEnumerator Charge()
    {
        Vector3 playerPos = Player.m_Instance.transform.position;

        rb.velocity = Vector2.zero;

        SpawnFireDrop();

        yield return new WaitForSeconds(m_ChargeDelay);

        m_Charging = true;
        Debug.Log("charging!");
        AudioManager.m_Instance.PlaySound(9);

        rb.velocity = (playerPos - transform.position).normalized * m_BurrowedSpeed;

        m_IsMidAnimation = true;

        StartCoroutine(Utils.DelayedCall(m_ChargeDuration, () =>
        {
            m_IsMidAnimation = false;
            m_Charging = false;
            Debug.Log("stopped charging");
        }));
    }

    private void ToggleBurrow(bool on)
    {
        m_Burrowed = on;
        GetComponentInChildren<SpriteRenderer>().enabled = !on;
        m_NormalCollider.enabled = !on;
        m_BurrowedCollider.enabled = on;
        m_DirtParticles.SetActive(on);
        m_DebuffPlacement.SetActive(!on);

        if (on)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            StartCoroutine(DropFire());
            m_DebuffImmune = true;
            Debug.Log("burrowed!");
            AudioManager.m_Instance.PlaySound(16);
        }
        else
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            StopCoroutine(DropFire());
            m_DebuffImmune = false;
            Debug.Log("unburrowed");
        }
    }

    private IEnumerator Shoot()
    {
        m_ProjectileOnCooldown = true;

        m_IsMidAnimation = true;

        GetComponentInChildren<Animator>().Play("Attack", -1, 0f);

        yield return new WaitForSeconds(0.45f);

        ProjectileManager.m_Instance.EnemyShot(m_Mouth.transform.position,
        Utils.GetDirectionToGameObject(m_Mouth.transform.position, Player.m_Instance.gameObject),
            m_ProjectileSpeed,
            m_ProjectileLifetime,
            m_ProjectilePrefab,
            m_ProjectileDamage,
            m_ProjectileKnockback,
            gameObject,
            DamageType.Fire);

        AudioManager.m_Instance.PlaySound(4);

        StartCoroutine(Utils.DelayedCall(0.1f, () =>
        {
            m_IsMidAnimation = false;
        }));

        yield return new WaitForSeconds(m_ProjectileCooldown);

        m_ProjectileOnCooldown = false;
    }

    protected override void FaceForward(Vector3 targetVelocity)
    {
        // If velocity > 0, don't flip. if it is less than, flip
        float faceDir = targetVelocity.x > 0 ? 1f : -1f;

        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * faceDir, transform.localScale.y);
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);

        GameObject otherObject = collision.gameObject;

        if (m_Stunned) return;
        if (!otherObject.GetComponent<Player>()) return;
        if (otherObject.GetComponent<Player>().m_IsInvincible) return;

        Debuff debuff = new Debuff(Debuff.DebuffType.Blaze, DamageType.Fire, 2f, 1, 3f, gameObject);
        DebuffManager.m_Instance.AddDebuff(collision.GetComponent<Actor>(), debuff);
    }
}