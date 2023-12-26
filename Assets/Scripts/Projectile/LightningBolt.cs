using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    float m_LengthModifier;

    private void Start()
    {
        m_LengthModifier = 1f / (transform.localScale.y * 0.925f);
    }
    private void Update()
    {
        Vector2 dir = GameplayManager.GetClosestEnemyPos(transform.position) - (Vector2)transform.position;

        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

        GetComponent<SpriteRenderer>().size = new Vector2(
            GetComponent<SpriteRenderer>().size.x,
            Vector2.Distance(transform.position, GameplayManager.GetClosestEnemyPos(transform.position) * m_LengthModifier)
            );
    }
}
