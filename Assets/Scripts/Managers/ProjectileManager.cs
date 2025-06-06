using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager m_Instance;

    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_SpinningBulletPrefab;

    [SerializeField] float m_BasicAttackScaling;
    [SerializeField] float m_BasicAttackLifetime;

    void Awake()
    {
        m_Instance = this;
    }

    #region Projectiles
    public GameObject Shoot(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime, GameObject bulletPrefab)
    {
        if (!bulletPrefab.GetComponent<Projectile>()) return null;

        // Create bullet from prefab
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.SetParent(transform);
        bullet.GetComponent<Projectile>().Init(pos, dir, speed, ability, lifetime);

        return bullet;
    }

    public GameObject Shoot(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        return Shoot(pos, dir, speed, ability, lifetime, m_BulletPrefab);
    }

    public GameObject EnemyShot(Vector2 pos, Vector2 dir, float speed, float lifetime, GameObject bulletPrefab, float damage, float knockback, GameObject user, DamageType damageType)
    {
        if (!bulletPrefab.GetComponent<NPCProjectile>()) return null;

        // Create bullet from prefab
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.SetParent(transform);
        bullet.GetComponent<NPCProjectile>().Init(pos, dir, speed, lifetime, damage, knockback, user, damageType);

        return bullet;
    }

    public GameObject ShootAOESpawningProjectile(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime, GameObject aoe, float aoeLifetime, GameObject bulletPrefab)
    {
        GameObject bullet = Shoot(pos, dir, speed, ability, lifetime, bulletPrefab);
        bullet.GetComponent<AOESpawningProjectile>().aoePrefab = aoe;
        bullet.GetComponent<AOESpawningProjectile>().aoeLifetime = aoeLifetime;

        return bullet;
    }

    public GameObject[] MultiShot(Vector2 pos, float speed, int numShots, Ability ability, float lifetime)
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

            bullets[i] = Shoot(pos, new Vector2(x,y), speed, ability, lifetime);
        }
        return bullets;
    }

    public GameObject ShootSpinning(float speed, Ability ability, float offset, float radius)
    {
        // Create bullet from prefab
        GameObject bullet = Instantiate(m_SpinningBulletPrefab);
        bullet.transform.SetParent(transform);

        SpinningProjectile bulletScript = bullet.GetComponent<SpinningProjectile>();

        bulletScript.Init(speed,ability,offset,radius);

        return bullet;
    }
    
    public GameObject[] ShootMultipleSpinning(float speed, Ability ability, float radius, int amount)
    {
        GameObject[] bullets = new GameObject[amount];

        float interval = 360 / amount;
        for (int i = 0; i < amount; i++)
        {
            // Create bullet from prefab
            bullets[i] = ShootSpinning(speed, ability, interval*i, radius);
        }

        return bullets;
    }
    #endregion

    public void EndTargetCooldown(GameObject enemy, float hitboxDelay, List<GameObject> hitTargets)
    {
        StartCoroutine(TargetCooldownRoutine(enemy, hitboxDelay, hitTargets));
    }

    private IEnumerator TargetCooldownRoutine(GameObject enemy, float hitboxDelay, List<GameObject> hitTargets)
    {
        yield return new WaitForSeconds(hitboxDelay);

        hitTargets.Remove(enemy);
    }
}