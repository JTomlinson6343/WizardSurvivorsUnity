using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum SpawnPoint
{
    Player,
    Staff
}

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager m_Instance;

    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_SpinningBulletPrefab;
    [SerializeField] GameObject m_FloatingDamagePrefab;

    void Awake()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot(SpawnPoint.Player,Vector2.one,1,Color.green,1,1);
        }
    }

    private float GetPlayerDamage()
    {
        return Player.m_Instance.GetStats().damage;
    }

    public void Shoot(Vector2 pos, Vector2 dir, float speed, Color colour, float damage, float lifetime)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_BulletPrefab);

        bullet.transform.SetParent(transform);
        bullet.GetComponent<Projectile>().m_Damage = damage * GetPlayerDamage();
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

    public void Shoot(SpawnPoint spawnPoint, Vector2 dir, float speed, Color colour, float damage, float lifetime)
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
        Shoot(pos,dir,speed, colour, damage, lifetime);
    }

    public void MultiShot(Vector2 pos, float speed, Color colour, int numShots, float damage, float lifetime)
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

            Shoot(pos, new Vector2(x,y), speed, colour, damage, lifetime);
        }
    }

    public void MultiShot(SpawnPoint spawnPoint, float speed, Color colour, int numShots, float damage, float lifetime)
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

            Shoot(spawnPoint, new Vector2(x,y), speed, colour, damage, lifetime);
        }
    }

    public void ShootSpinning(float speed, Color colour, float damage, float offset, float radius)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_SpinningBulletPrefab);
        bullet.transform.SetParent(transform);

        SpinningProjectile bulletScript = bullet.GetComponent<SpinningProjectile>();

        bulletScript.m_Damage = damage * GetPlayerDamage();
        bulletScript.speed = speed;
        bulletScript.offset = offset;
        bulletScript.radius = radius;
        bulletScript.Init();
    }
    
    public void ShootMultipleSpinning(float speed, Color colour, float damage, float radius, int amount)
    {
        float interval = 360 / amount;
        for (int i = 0; i < amount; i++)
        {
            // Create bullet from prefab
            ShootSpinning(speed, colour, damage, interval*i, radius);
        }
    }
}
