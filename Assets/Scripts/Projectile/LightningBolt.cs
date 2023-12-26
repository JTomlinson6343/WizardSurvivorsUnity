using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : AOEObject
{
    float m_LengthModifier;

    private void Start()
    {
        m_LengthModifier = 1f / (transform.localScale.y * 0.9f);
    }
    private void Update()
    {
        Vector2 dir = GameplayManager.GetDirectionToClosestEnemy(transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

        //Vector2 enemyPos = GameplayManager.GetGameObjectCentre(GameplayManager.GetClosestEnemy(transform.position).gameObject);
        Vector2 enemyPos = GameplayManager.GetClosestEnemyPos(transform.position);

        if (enemyPos == Vector2.negativeInfinity) return;

        GetComponent<SpriteRenderer>().size = new Vector2(
            GetComponent<SpriteRenderer>().size.x,
            Vector2.Distance(transform.position, enemyPos * m_LengthModifier)
            );
    }
}
