using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlySkeleton : Actor
{
    GameObject m_TargetedEnemy = null;
    [SerializeField] float m_Range = 4f;
    [SerializeField] float m_Speed = 2f;

    override public void Update()
    {
        base.Update();

        if (m_TargetedEnemy == null)
        {
            m_TargetedEnemy = Utils.GetClosestEnemyInRange(transform.position, m_Range);
        }

        if (m_TargetedEnemy == null) return;

        Vector2 dir = (m_TargetedEnemy.transform.position - transform.position).normalized;

        GetComponent<Rigidbody2D>().velocity = dir * m_Speed;
    }
    public void TargetEnemy(GameObject enemy)
    {
        m_TargetedEnemy = enemy;
    }

    public void CrawlFromGround()
    {
        Animator animator = GetComponent<Animator>();
        PlayMethodAfterAnimation("Uproot", 0.25f, nameof(EndCrawl));
    }

    private void EndCrawl()
    {
        m_IsMidAnimation = false;
    }
}
