using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BlackHoleAOE : DebuffAOE
{
    readonly float kPullSpeedConst = 0.01f;
    readonly float kHitboxShinkAmount = 0.3f;
    [SerializeField] float m_SuckSpeed;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerStay2D(collision);

        // Pull all enemies in range into the centre
        Vector3 towardsCentre = (transform.position - collision.transform.position).normalized;

        collision.gameObject.transform.position += towardsCentre * kPullSpeedConst * m_SuckSpeed;
    }
}
