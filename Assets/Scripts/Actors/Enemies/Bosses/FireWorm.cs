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
    [SerializeField] float m_BurrowedSpeed;
    [SerializeField] float m_MinBurrowDuration;
    [SerializeField] float m_MaxBurrowDuration;
    [SerializeField] float m_ChargeDelay;
    bool m_Charging;

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

        rb.velocity = Utils.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject) * -m_Speed;
    }

    private void BurrowCheck() // Try and burrow if the rng value is correct
    {
        if (Random.Range(0f, 1f) <= m_BurrowChance)
        {
            ToggleBurrow(true);
            StartCoroutine(Burrow());
        }
    }

    private IEnumerator Burrow()
    {
        Debug.Log("burrowed!");
        float duration = Random.Range(m_MinBurrowDuration, m_MaxBurrowDuration);
        float timeLeft = duration;

        GetComponentInChildren<SpriteRenderer>().color = Color.green;

        yield return Charge();

        //while (timeLeft > 0f)
        //{
        //    if (!PlayerManager.m_Instance.m_BossArenaBounds.IsInBounds(m_Mouth.transform.position, 0.8f) && !m_Charging)
        //    {
        //        yield return Charge();
        //    }

        //    timeLeft -= Time.deltaTime;
        //    yield return new WaitForEndOfFrame();
        //}

        ToggleBurrow(false);
        m_BurrowOnCooldown = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        yield return new WaitForSeconds(m_BurrowCooldown);

        m_BurrowOnCooldown = false;
    }

    private IEnumerator Charge()
    {
        m_Charging = true;

        Vector3 playerPos = Player.m_Instance.transform.position;

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(m_ChargeDelay);

        m_Charging = false;

        rb.velocity = (playerPos - transform.position).normalized * m_BurrowedSpeed;
    }

    private void ToggleBurrow(bool on)
    {
        m_Burrowed = on;
        if (on)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        else
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private Vector2 PickRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        return new Vector2(x, y).normalized;
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

        AudioManager.m_Instance.PlaySound(15);

        StartCoroutine(Utils.DelayedCall(0.1f, () =>
        {
            m_IsMidAnimation = false;
        }));

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