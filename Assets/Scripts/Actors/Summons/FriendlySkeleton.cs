using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlySkeleton : Actor
{
    public Ability m_AbilitySource;
    GameObject m_TargetedEnemy = null;
    List<GameObject> m_HitTargets = new List<GameObject>();

    [SerializeField] Animator m_Animator;
    [SerializeField] Animator m_AnimatorMask;
    [SerializeField] float m_Range = 4f;
    [SerializeField] float m_PlayerMinDist = 1f;
    [SerializeField] float m_Speed = 2f;
    float m_Duration = 8f;
    [SerializeField] float m_HitboxDelay = 0.25f;

    [SerializeField] GameObject m_DeathParticlesPrefab;

    public void Init(float duration)
    {
        m_Duration = duration;
        CrawlFromGround();
        m_MaxHealth = m_MaxHealth = m_Duration;
        m_Health = m_MaxHealth;
    }

    override public void Update()
    {
        base.Update();

        if (m_Health > 0f)
        {
            m_Health -= Time.deltaTime;
        }
        else
        {
            OnDeath();
            return;
        }

        if (m_IsMidAnimation || Vector2.Distance(Player.m_Instance.transform.position, transform.position) < m_PlayerMinDist)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        if (!m_TargetedEnemy)
        {
            m_TargetedEnemy = Utils.GetClosestEnemyInRange(transform.position, m_Range);

            if (!m_TargetedEnemy)
            {
                Vector2 pDir = (Player.m_Instance.transform.position - transform.position).normalized;
                FaceForward(pDir);

                GetComponent<Rigidbody2D>().velocity = pDir * m_Speed;
                return;
            }
        }

        Vector2 dir = (m_TargetedEnemy.transform.position - transform.position).normalized;
        FaceForward(dir);

        GetComponent<Rigidbody2D>().velocity = dir * m_Speed;
    }

    virtual protected void FaceForward(Vector3 targetVelocity)
    {
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();

        // If velocity > 0, don't flip. if it is less than, flip
        float faceDir = targetVelocity.x > 0 ? 1f : -1f;

        sprite.transform.localScale = new Vector2(Mathf.Abs(sprite.transform.localScale.x) * faceDir, sprite.transform.localScale.y);
    }

    public void TargetEnemy(GameObject enemy)
    {
        m_TargetedEnemy = enemy;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        if (!otherObject.GetComponent<Enemy>()) return;

        OnTargetHit(otherObject);
    }

    void OnTargetHit(GameObject enemy)
    {
        if (m_HitTargets.Contains(enemy)) return;
        m_HitTargets.Add(enemy);

        ProjectileManager.m_Instance.EndTargetCooldown(enemy, m_HitboxDelay, m_HitTargets);

        DamageTarget(enemy);
    }

    void DamageTarget(GameObject enemy)
    {
        DamageInstanceData data = new DamageInstanceData(Player.m_Instance.gameObject, enemy);
        data.amount = m_AbilitySource.GetTotalStats().damage;
        data.damageType = DamageType.Dark;
        data.doDamageNumbers = true;
        data.abilitySource = m_AbilitySource;

        DamageManager.m_Instance.DamageInstance(data, transform.position);
    }

    public void CrawlFromGround()
    {
        PlayMethodAfterAnimation("Uproot", 0.25f, nameof(EndCrawl));
    }

    private void EndCrawl()
    {
        m_IsMidAnimation = false;

        Animator animator = GetComponentInChildren<Animator>();
        m_Animator?.SetBool("Moving", true);
        m_AnimatorMask?.SetBool("Moving", true);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        GameObject smoke = Instantiate(m_DeathParticlesPrefab);
        smoke.transform.position = transform.position;
        // TODO: Explode
    }
}
