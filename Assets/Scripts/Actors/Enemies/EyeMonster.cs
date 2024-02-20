using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMonster : Enemy
{
    [SerializeField] float m_ShootCooldown;
    private bool m_CanShoot;
    [SerializeField] float m_ShootRange;
    [SerializeField] GameObject m_ShootPos;
    [SerializeField] float m_ProjectileSpeed;
    [SerializeField] float m_ProjectileDamage;
    [SerializeField] float m_ProjectileLifetime;

    [SerializeField] GameObject m_ProjectilePrefab;

    public override void Update()
    {
        base.Update();

        if (m_CanShoot && Vector2.Distance(transform.position, Player.m_Instance.transform.position) < m_ShootRange)
            Shoot();
    }

    private void Shoot()
    {
        Vector2 dir = GameplayManager.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject);

        ProjectileManager.m_Instance.EnemyShot(m_ShootPos.transform.position,
            dir,
            m_ProjectileSpeed,
            m_ProjectileLifetime,
            m_ProjectilePrefab,
            m_ProjectileDamage,
            0f,
            gameObject,
            DamageType.Dark);

        StartShootCooldown();
    }

    private IEnumerator StartShootCooldown()
    {
        yield return new WaitForSeconds(m_ShootCooldown);
        m_CanShoot = true;
    }
}
