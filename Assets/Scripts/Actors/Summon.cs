using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed;
    [SerializeField] float m_LeashRange;
    [SerializeField] float m_RangeCheckDelay;

    [SerializeField] float m_AttackRange;
    [SerializeField] float m_ProjectileSpeed;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector2.zero, m_LeashRange);
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
    }

    private void Update()
    {
        Brain();
    }

    private void Brain()
    {
        // Check if out of range of the player
        if (Vector2.Distance(transform.position, Player.m_Instance.transform.position) > m_LeashRange)
        {
            GoToPlayer();
        }
        //Attack();
    }

    private void GoToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.m_Instance.transform.position, m_MoveSpeed/100f);
    }

    //private void Attack()
    //{
    //    GameObject enemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AttackRange);

    //    ProjectileManager.m_Instance.Shoot(
    //        transform.position,
    //        GameplayManager.GetDirectionToGameObject(transform.position,enemy),
    //        m_ProjectileSpeed,

    //        )
    //}


}
