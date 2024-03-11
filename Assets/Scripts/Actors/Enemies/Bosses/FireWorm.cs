using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : Boss
{
    [SerializeField] GameObject m_Mouth;

    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileDamage;
    [SerializeField] float m_ProjectileKnockback;
    [SerializeField] float m_ProjectileCooldown;

    [SerializeField] float m_MinShootRange; // Min range the worm needs to be to shoot the player

    [SerializeField] bool m_Burrowed;
    [SerializeField] float m_BurrowChance;
    [SerializeField] float m_BurrowCooldown;
    [SerializeField] float m_MinBurrowDuration;
    [SerializeField] float m_MaxBurrowDuration;

    private bool m_ProjectileOnCooldown;
    private bool m_BurrowOnCooldown;

    public override void Enraged(int bossNumber)
    {
        throw new System.NotImplementedException();
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

        float distToPlayer = Vector2.Distance(Player.m_Instance.transform.position, transform.position);

        if (!m_Burrowed)
        {
            if (!m_BurrowOnCooldown) BurrowCheck();

            if (distToPlayer < m_MinShootRange)
            {
                MoveAwayRoutine();
                if (!m_IsMidAnimation)
                {
                    GetComponentInChildren<Animator>().Play("Moving");
                    FaceForward(rb.velocity);
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (!m_ProjectileOnCooldown) StartCoroutine(Shoot());
                if (!m_IsMidAnimation)
                {
                    GetComponentInChildren<Animator>().Play("Idle");
                    FaceForward(Player.m_Instance.transform.position - transform.position);
                }
            }
        }
    }

    private void MoveAwayRoutine()
    {
        if (!PlayerManager.m_Instance.m_BossArenaBounds.IsInBounds(m_Mouth.transform.position))
        {
            StartCoroutine(Burrow());
        }

        rb.velocity = GameplayManager.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject) * -m_Speed;
    }

    private void BurrowCheck() // Try and burrow if the rng value is correct
    {
        //if (Random.Range(0f, 1f) <= m_BurrowChance)
        //{
        //    StartCoroutine(Burrow());
        //}
    }

    private IEnumerator Burrow()
    {
        m_Burrowed = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float duration = Random.Range(m_MinBurrowDuration, m_MaxBurrowDuration);
        float timeLeft = duration;

        GetComponentInChildren<SpriteRenderer>().color = Color.green;

        while (timeLeft > 0f)
        {
            rb.velocity = GameplayManager.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject) * m_Speed;

            timeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        m_Burrowed = false;
        m_BurrowOnCooldown = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        yield return new WaitForSeconds(m_BurrowCooldown);

        m_BurrowOnCooldown = false;
    }

    private IEnumerator Shoot()
    {
        m_ProjectileOnCooldown = true;

        m_IsMidAnimation = true;

        GetComponentInChildren<Animator>().Play("Attack", -1, 0f);

        yield return new WaitForSeconds(0.45f);

        m_IsMidAnimation = false;
        ProjectileManager.m_Instance.EnemyShot(m_Mouth.transform.position,
        GameplayManager.GetDirectionToGameObject(m_Mouth.transform.position, Player.m_Instance.gameObject),
            m_ProjectileSpeed,
            m_ProjectileLifetime,
            m_ProjectilePrefab,
            m_ProjectileDamage,
            m_ProjectileKnockback,
            gameObject,
            DamageType.Fire);

        AudioManager.m_Instance.PlaySound(15);

        yield return new WaitForSeconds(m_ProjectileCooldown);

        m_ProjectileOnCooldown = false;
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