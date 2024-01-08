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

    [SerializeField] float m_StompCooldown;

    private bool m_ProjectileOnCooldown;
    private bool m_TeleportOnCooldown;
    private bool m_StompOnCooldown;

    [SerializeField] GameObject m_Staff;
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
            StateManager.ChangeState(State.BOSS);
            Player.m_Instance.transform.position = Vector3.zero;
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
        Animator animator = GetComponentInChildren<Animator>();
        if (Player.m_Instance == null) return;

        float distToPlayer = Vector2.Distance(Player.m_Instance.transform.position, transform.position);

        if (distToPlayer < m_MeleeRadius)
        {
            // Stomp attack
        }
        else
        {
            TeleportCheck();

            if (m_ProjectileOnCooldown) return;

            //Ranged attack
            animator.Play("Magic", -1, 0f);
            Invoke(nameof(Shoot), 0.5f);
            m_ProjectileOnCooldown = true;
        }
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

        Invoke(nameof(EndProjectileCooldown), m_ProjectileCooldown);
        GetComponentInChildren<Animator>().Play("MagicDown", -1, 0f);
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
        Invoke(nameof(EndTeleportCooldown), m_TeleportCooldown);

        SpawnSmoke();
        Invoke(nameof(Reappear), 2f);
        transform.position = Player.m_Instance.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * m_TeleportRadius;
        gameObject.SetActive(false);
    }

    private void SpawnSmoke()
    {
        GameObject smoke = Instantiate(m_SmokePrefab);
        smoke.transform.position = m_DebuffPlacement.transform.position;
    }

    private void Reappear()
    {
        SpawnSmoke();
        gameObject.SetActive(true);
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
