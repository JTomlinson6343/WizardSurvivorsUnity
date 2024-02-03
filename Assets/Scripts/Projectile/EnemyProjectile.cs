using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : NPCProjectile
{
    protected override void OnTargetHit(GameObject target)
    {
        if (target.GetComponent<Player>().m_IsInvincible) return;

        base.OnTargetHit(target);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.isTrigger)
        {
            OnTargetHit(collision.gameObject);
        }
    }
}
