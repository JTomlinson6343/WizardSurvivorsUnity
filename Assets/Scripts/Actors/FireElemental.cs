using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemental : Summon
{
    [SerializeField] GameObject m_FlamethrowerObject;

    private void Start()
    {
        m_FlamethrowerObject.GetComponentInChildren<DebuffAOE>().m_AbilitySource = m_AbilitySource;
    }

    protected override void Attack()
    {
        GameObject closestEnemy = GameplayManager.GetClosestEnemyInRange(transform.position, 4.25f);

        if (!closestEnemy)
        {
            DisableFlames();
            return;
        }

        m_FlamethrowerObject.SetActive(true);

        Vector2 dir = GameplayManager.GetDirectionToGameObject(transform.position, closestEnemy);

        m_FlamethrowerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }

    private void DisableFlames()
    {
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
