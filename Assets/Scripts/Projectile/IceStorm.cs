using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IceStorm : ConstantDamageAOE
{
    [SerializeField] float m_SlowSpeedModifier;
    bool m_IsSlowed;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerEnter2D(collision);

        if (m_IsSlowed) return;

        GetComponent<Rigidbody2D>().velocity *= m_SlowSpeedModifier;

        m_IsSlowed = true;
    }
}