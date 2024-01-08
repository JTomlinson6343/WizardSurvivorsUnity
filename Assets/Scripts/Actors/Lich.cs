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

    private bool m_ProjectileOnCooldown;

    [SerializeField] GameObject m_Staff;
    [SerializeField] GameObject m_ProjectilePrefab;
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

        Brain();
    }

    private void Brain()
    {
        if (Player.m_Instance == null) return;

        float distToPlayer = Vector2.Distance(Player.m_Instance.transform.position, transform.position);

        if (distToPlayer < m_MeleeRadius)
        {
            // Stomp attack
        }
        else
        {
            if (m_ProjectileOnCooldown)  return; 

            //Ranged attack
            ProjectileManager.m_Instance.EnemyShot(m_Staff.transform.position,
                GameplayManager.GetDirectionToGameObject(m_Staff.transform.position, Player.m_Instance.gameObject),
                m_ProjectileSpeed,
                m_ProjectileLifetime,
                m_ProjectilePrefab,
                m_ProjectileDamage,
                m_ProjectileKnockback,
                gameObject,
                DamageType.Dark);
            m_ProjectileOnCooldown = true;
            Invoke(nameof(EndProjectileCooldown), m_ProjectileCooldown);
        }
    }

    private void EndProjectileCooldown()
    {
        m_ProjectileOnCooldown = false;
    }
}
