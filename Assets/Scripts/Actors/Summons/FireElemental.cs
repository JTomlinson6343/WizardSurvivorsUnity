using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FireElemental : Summon
{
    [SerializeField] GameObject m_FlamethrowerObject;

    private void Start()
    {
        m_FlamethrowerObject.GetComponentInChildren<DebuffAOE>().m_AbilitySource = m_AbilitySource;
    }

    protected override void Attack()
    {
        GameObject closestEnemy = GameplayManager.GetClosestEnemyInRange(transform.position, m_AbilitySource.m_DefaultAutofireRange);

        if (!closestEnemy)
        {
            DisableFlames();
            return;
        }

        m_FlamethrowerObject.SetActive(true);

        Vector2 dir = GameplayManager.GetDirectionToGameObject(transform.position, closestEnemy);

        FaceEnemy(closestEnemy.transform.position);

        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }

    private void FaceEnemy(Vector2 enemyPos)
    {
        if (enemyPos.x > transform.position.x)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        particles.transform.localScale = new Vector3(transform.localScale.x, particles.transform.localScale.y, particles.transform.localScale.z);
    } 
    private void DisableFlames()
    {
        if (m_FlamethrowerObject.activeSelf)
        {
            StartCoroutine(AttackLockout());
        }
        m_FlamethrowerObject.SetActive(false);
    }
    protected override IEnumerator GoToPlayer()
    {
        DisableFlames();
        return base.GoToPlayer();
    }

    protected override void GoToEnemy()
    {
        DisableFlames();
        base.GoToEnemy();
    }
}
