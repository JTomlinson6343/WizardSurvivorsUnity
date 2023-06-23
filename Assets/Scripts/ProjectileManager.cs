using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager m_Instance;

    [SerializeField] GameObject m_BulletPrefab;

    void Awake()
    {
        m_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MultiShot(new Vector2(0, 0), 10, Color.blue, 4, 1, 1.0f);
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

        bullet.GetComponent<Projectile>().damage = damage * GetPlayerDamage();
        bullet.GetComponent<Projectile>().StartLifetimeTimer(lifetime);

        // Set pos and velocity of bullet
        bullet.transform.position = pos;
        bullet.transform.GetComponent<Rigidbody2D>().velocity = dir * speed;

        // Set colour of light
        bullet.transform.GetComponent<Light2D>().color = colour;
    }

    public void MultiShot(Vector2 pos, float speed, Color colour, int numShots, float damage, float lifetime)
    {
        float interval = 360 / numShots;

        float offset = interval / 2;

        for (int i = 0; i < numShots; i++)
        {
            float angle = (interval * i + offset) * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            Shoot(pos, new Vector2(x,y), speed, colour, damage, lifetime);
        }
    }
}
