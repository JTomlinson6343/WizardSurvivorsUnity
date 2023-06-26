using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] Color m_BasicAttackColour;
    [SerializeField] float m_BasicAttackLifetime;

    void Awake()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootDir = (m_CameraRef.ScreenToWorldPoint(Input.mousePosition) - Player.m_Instance.GetStaffTransform().position).normalized;

            Shoot(Player.m_Instance.GetStaffTransform().position, shootDir.normalized, Player.m_Instance.GetStats().shotSpeed, m_BasicAttackColour, m_BasicAttackScaling, m_BasicAttackLifetime);
        }
    }

    private float GetPlayerDamage()
    {
        return Player.m_Instance.GetStats().damage;
    }

    public void Shoot(Vector2 pos, Vector2 dir, float speed, Color colour, float damageScaling, float lifetime)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_BulletPrefab);

        bullet.transform.SetParent(transform);
        bullet.GetComponent<Projectile>().m_Damage = damageScaling * GetPlayerDamage();
        bullet.GetComponent<Projectile>().StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        bullet.transform.position = pos;

        Rigidbody2D rb = bullet.transform.GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;

        // Rotate projectile in direction of travel
        bullet.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90);

        // Set colour of light
        bullet.transform.GetComponent<Light2D>().color = colour;
    }

    public void Shoot(SpawnPoint spawnPoint, Vector2 dir, float speed, Color colour, float damageScaling, float lifetime)
    {
        Vector2 pos = Vector2.zero;
        // Set pos of bullet
        switch (spawnPoint)
        {
            case SpawnPoint.Player:
                pos = Player.m_Instance.GetPosition();
                break;
            case SpawnPoint.Staff:
                pos = Player.m_Instance.GetStaffTransform().position;
                break;
            default:
                break;
        }
        // Call shoot function
        Shoot(pos,dir,speed, colour, damageScaling, lifetime);
    }

    public void MultiShot(Vector2 pos, float speed, Color colour, int numShots, float damageScaling, float lifetime)
    {
        // How many degrees separate each shot
        float interval = 360 / numShots;

        // How far from 0 degrees to start spawning shots
        float offset = interval / 2;

        for (int i = 0; i < numShots; i++)
        {
            float angle = (interval * i + offset) * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            Shoot(pos, new Vector2(x,y), speed, colour, damageScaling, lifetime);
        }
    }

    public void MultiShot(SpawnPoint spawnPoint, float speed, Color colour, int numShots, float damageScaling, float lifetime)
    {
        // How many degrees separate each shot
        float interval = 360 / numShots;

        // How far from 0 degrees to start spawning shots
        float offset = interval / 2;

        for (int i = 0; i < numShots; i++)
        {
            float angle = (interval * i + offset) * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            Shoot(spawnPoint, new Vector2(x,y), speed, colour, damageScaling, lifetime);
        }
    }

    public void ShootSpinning(float speed, Color colour, float damageScaling, float offset, float radius)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_SpinningBulletPrefab);
        bullet.transform.SetParent(transform);

        SpinningProjectile bulletScript = bullet.GetComponent<SpinningProjectile>();

        bulletScript.m_Damage = damageScaling * GetPlayerDamage();
        bulletScript.speed = speed;
        bulletScript.offset = offset;
        bulletScript.radius = radius;
        bulletScript.Init();
    }
    
    public void ShootMultipleSpinning(float speed, Color colour, float damageScaling, float radius, int amount)
    {
        float interval = 360 / amount;
        for (int i = 0; i < amount; i++)
        {
            // Create bullet from prefab
            ShootSpinning(speed, colour, damageScaling, interval*i, radius);
        }
    }
}
