using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : Boss
{
    [SerializeField] float m_MeleeRadius;
    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] float m_ProjectileLifetime;
    [SerializeField] float m_ProjectileDamage;
    [SerializeField] float m_ProjectileKnockback;
    [SerializeField] float m_ProjectileCooldown;

    [SerializeField] float m_TeleportCooldown;
    [SerializeField] float m_TeleportChancePerFrame;
    [SerializeField] float m_MinTeleportRadius; // Min distance from the player the teleport pos can be
    [SerializeField] float m_MaxTeleportRadius; // Max distance from the player the teleport pos can be
    [SerializeField] float m_TeleportVanishDuration;

    [SerializeField] float m_StompCooldown;
    [SerializeField] float m_StompDamage;
    [SerializeField] int   m_EnemiesSpawned;
    [SerializeField] float m_EnemySpawnRadius;

    private bool m_ProjectileOnCooldown;
    private bool m_TeleportOnCooldown;
    private bool m_StompOnCooldown;

    [SerializeField] GameObject m_Staff;
    [SerializeField] GameObject m_QuakePos;
    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] GameObject m_QuakePrefab;
    [SerializeField] GameObject m_SmokePrefab;
    [SerializeField] GameObject m_SpawnedEnemyPrefab;

    public override void Enraged(int bossNumber)
    {
        m_MaxHealth *= bossNumber;
        m_StompDamage *= 1f + bossNumber * 0.1f;
        m_ProjectileDamage *= 1f + bossNumber * 0.1f;
        m_EnemiesSpawned += bossNumber - 1;
        m_ProjectileCooldown *= 1f/bossNumber;
        m_ProjectileSpeed *= 1f + bossNumber * 0.05f;

        m_BossName = "Enraged " + m_BossName;

        GetComponentInChildren<SpriteRenderer>().color = Color.gray;
    }

    public override void Update()
    {
        if (StateManager.GetCurrentState() != State.BOSS) return;

        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        // Turn to face player
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
            // If enemy is in range of player, use stomp attack
            PlayMethodAfterAnimation("Stomp", 1.45f, nameof(Stomp), ref m_StompOnCooldown);
        }
        else
        {
            // If player is out of melee range, try and teleport.
            TeleportCheck();

            //Ranged attack
            PlayMethodAfterAnimation("Magic", 0.5f, nameof(Shoot), ref m_ProjectileOnCooldown);
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

        for (int i = 0; i < m_EnemiesSpawned; i++)
        {
            GameObject spawnedEnemy = EnemyManager.m_Instance.CreateNewEnemy(m_SpawnedEnemyPrefab);
            spawnedEnemy.transform.position = m_QuakePos.transform.position + GameplayManager.GetRandomDirectionV3() * m_EnemySpawnRadius;
            spawnedEnemy.GetComponent<Skeleton>().CrawlFromGround();
        }
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
        m_ProjectileOnCooldown = false;
        m_StompOnCooldown = false;

        Vector2 newPos;
        while (true)
        {
            newPos = Player.m_Instance.transform.position + GameplayManager.GetRandomDirectionV3() * m_MinTeleportRadius;

            if (PlayerManager.m_Instance.m_BossArenaBounds.IsInBounds(newPos)) break;
        }

        transform.position = newPos;
        SpawnSmoke();
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
