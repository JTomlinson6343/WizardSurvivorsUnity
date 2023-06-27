using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering.Universal;
using static UnityEditor.PlayerSettings;

public enum SpawnPoint
{
    Player,
    Staff
}

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager m_Instance;

    [SerializeField] Camera     m_CameraRef;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_SpinningBulletPrefab;
    [SerializeField] GameObject m_FloatingDamagePrefab;

    Vector2 shootDir;

    [SerializeField] float m_BasicAttackScaling;
    [SerializeField] float m_BasicAttackLifetime;

    float m_LastShot = 0;

    void Awake()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float now = Time.realtimeSinceStartup;

            if (now - m_LastShot > Player.m_Instance.GetFireDelay())
            {
                BasicAttack();
                m_LastShot = now;
            }
        }
    }

    private void BasicAttack()
    {
        shootDir = (m_CameraRef.ScreenToWorldPoint(Input.mousePosition) - Player.m_Instance.GetStaffTransform().position).normalized;

        Shoot(Player.m_Instance.GetStaffTransform().position, shootDir.normalized, Player.m_Instance.GetStats().shotSpeed, m_BasicAttackScaling, m_BasicAttackLifetime);

    }

    private float GetPlayerDamage()
    {
        return Player.m_Instance.GetStats().damage;
    }

    // Function used to spawn bullets at a pre-defined point
    private Vector2 GetSpawnPoint(SpawnPoint spawnPoint)
    {
        switch (spawnPoint)
        {
            case SpawnPoint.Player:
                return Player.m_Instance.GetPosition();
            case SpawnPoint.Staff:
                return Player.m_Instance.GetStaffTransform().position;
            default:
                return Vector2.zero;
        }
    }

    public GameObject Shoot(Vector2 pos, Vector2 dir, float speed, float damageScaling, float lifetime)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_BulletPrefab);

        bullet.transform.SetParent(transform);
        bullet.GetComponent<Projectile>().m_Damage = damageScaling;
        bullet.GetComponent<Projectile>().StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        bullet.transform.position = pos;

        Rigidbody2D rb = bullet.transform.GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;

        // Rotate projectile in direction of travel
        bullet.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90);

        return bullet;
    }

    public GameObject[] MultiShot(Vector2 pos, float speed, int numShots, float damageScaling, float lifetime)
    {
        GameObject[] bullets = new GameObject[numShots];
        // How many degrees separate each shot
        float interval = 360 / numShots;

        // How far from 0 degrees to start spawning shots
        float offset = interval / 2;

        for (int i = 0; i < numShots; i++)
        {
            float angle = (interval * i + offset) * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            bullets[i] = Shoot(pos, new Vector2(x,y), speed, damageScaling, lifetime);
        }
        return bullets;
    }

    public GameObject ShootSpinning(float speed, float damageScaling, float offset, float radius)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_SpinningBulletPrefab);
        bullet.transform.SetParent(transform);

        SpinningProjectile bulletScript = bullet.GetComponent<SpinningProjectile>();

        bulletScript.m_Damage = damageScaling;
        bulletScript.speed = speed;
        bulletScript.offset = offset;
        bulletScript.radius = radius;
        bulletScript.Init();

        return bullet;
    }
    
    public GameObject[] ShootMultipleSpinning(float speed, float damageScaling, float radius, int amount)
    {
        GameObject[] bullets = new GameObject[amount];

        float interval = 360 / amount;
        for (int i = 0; i < amount; i++)
        {
            // Create bullet from prefab
            bullets[i] = ShootSpinning(speed, damageScaling, interval*i, radius);
        }

        return bullets;
    }
}
