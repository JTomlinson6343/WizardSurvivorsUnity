using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAOE : DebuffAOE
{
    readonly float pullSpeedConst = 0.01f;

    public new Ability m_AbilitySource;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerStay2D(collision);

        Vector3 towardsCentre = (transform.position - collision.transform.position).normalized;

        collision.gameObject.transform.position += towardsCentre * pullSpeedConst * 5f;
    }
    override public void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (!rb) return;

        rb.isKinematic = true;

    }

    override public void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (!rb) return;

        rb.isKinematic = false;
    }
}
