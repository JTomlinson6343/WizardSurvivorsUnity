using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : Enemy
{
    [SerializeField] float m_MeleeRadius;
    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileDamage;
    [SerializeField] float m_ProjectileKnockback;
    [SerializeField] float m_ProjectileCooldown;

    [SerializeField] float m_TeleportCooldown;
    [SerializeField] float m_TeleportChancePerFrame;
    [SerializeField] float m_TeleportRadius; // Distance from the player the teleport pos can be
    [SerializeField] float m_TeleportVanishDuration;

    [SerializeField] float m_StompCooldown;
    [SerializeField] float m_StompDamage;

    private bool m_ProjectileOnCooldown;
    private bool m_TeleportOnCooldown;
    private bool m_StompOnCooldown;

    private bool m_IsMidAnimation;

    [SerializeField] GameObject m_Staff;
    [SerializeField] GameObject m_QuakePos;
    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] GameObject m_QuakePrefab;
    [SerializeField] GameObject m_SmokePrefab;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_MeleeRadius);
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ProgressionManager.m_Instance.SpawnBoss();
        }

        if (StateManager.GetCurrentState() != State.BOSS) return;

        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        if (Player.m_Instance.transform.position.x < transform.position.x)
        {
            sprite.transform.localScale = new Vector2(Mathf.Abs(sprite.transform.localScale.x) * -1f, sprite.transform.localScale.y);
        }
        else
        {
            sprite.transform.localScale = new Vector2(Mathf.Abs(sprite.transform.localScale.x), sprite.transform.localScale.y);
        }

        Brain();
    }

    private void Brain()
    {
        if (Player.m_Instance == null) return;
        if (m_IsMidAnimation) return;

        float distToPlayer = Vector2.Distance(Player.m_Instance.transform.position, transform.position);

        if (distToPlayer < m_MeleeRadius)
        {
            PlayMethodAfterAnimation("Stomp", 1.4f, nameof(Stomp), ref m_StompOnCooldown);
        }
        else
        {
            TeleportCheck();

            //Ranged attack
            PlayMethodAfterAnimation("Magic", 0.5f, nameof(Shoot), ref m_ProjectileOnCooldown);
        }
    }

    // Method called after 'delay' seconds and only if it is off cooldown
    private void PlayMethodAfterAnimation(string animation, float delay, string methodOnPlay, ref bool cooldownCheck)
    {
        Animator animator = GetComponentInChildren<Animator>();

        if (cooldownCheck) return;

        animator.Play(animation, -1, 0f);
        m_IsMidAnimation = true;
        Invoke(methodOnPlay, delay);
        cooldownCheck = true;
    }

    private void Shoot()
    {
        ProjectileManager.m_Instance.EnemyShot(m_Staff.transform.position,
            GameplayManager.GetDirectionToGameObject(m_Staff.transform.position, Player.m_Instance.gameObject),
            m_ProjectileSpeed,
            m_ProjectileLifetime,
            m_ProjectilePrefab,
            m_ProjectileDamage,
            m_ProjectileKnockback,
            gameObject,
            DamageType.Dark);

        AudioManager.m_Instance.PlaySound(15);

        Invoke(nameof(EndProjectileCooldown), m_ProjectileCooldown);
        GetComponentInChildren<Animator>().Play("MagicDown", -1, 0f);
        m_IsMidAnimation = false;
    }

    private void Stomp()
    {
        GameObject quake = Instantiate(m_QuakePrefab);
        quake.GetComponent<EnemyAOE>().Init(
            m_QuakePos.transform.position,
            m_StompDamage,
            2f,
            1.2f,
            gameObject,
            DamageType.Physical);;

        AudioManager.m_Instance.PlaySound(14);

        Invoke(nameof(EndStompCooldown), m_StompCooldown);
        GetComponentInChildren<Animator>().Play("AfterStomp", -1, 0f);
        m_IsMidAnimation = false;
    }

    private void TeleportCheck()
    {
        if (m_TeleportOnCooldown) return;

        if (Random.Range(0f, 1f) > m_TeleportChancePerFrame) return;

        print("Teleported");
        Teleport();
    }

    private void Teleport()
    {
        m_TeleportOnCooldown = true;
        m_ProjectileOnCooldown = true;
        m_StompOnCooldown = true;
        Invoke(nameof(EndTeleportCooldown), m_TeleportCooldown);

        SpawnSmoke();
        Invoke(nameof(Reappear), m_TeleportVanishDuration);
        transform.position = Player.m_Instance.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * m_TeleportRadius;
        gameObject.SetActive(false);

        m_IsMidAnimation = true;
    }

    private void SpawnSmoke()
    {
        GameObject smoke = Instantiate(m_SmokePrefab);
        smoke.transform.position = m_DebuffPlacement.transform.position;
        AudioManager.m_Instance.PlaySound(16);
    }

    private void Reappear()
    {
        SpawnSmoke();
        m_ProjectileOnCooldown = false;
        m_StompOnCooldown = false;
        gameObject.SetActive(true);
        m_IsMidAnimation = false;
    }

    private void EndProjectileCooldown()
    {
        m_ProjectileOnCooldown = false;
    }
    private void EndTeleportCooldown()
    {
        m_TeleportOnCooldown = false;
    }
    private void EndStompCooldown()
    {
        m_StompOnCooldown = false;
    }
}
