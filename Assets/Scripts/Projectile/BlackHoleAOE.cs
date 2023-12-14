using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BlackHoleAOE : DebuffAOE
{
    readonly float kPullSpeedConst = 0.01f;
    [SerializeField] float m_SuckSpeed;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerStay2D(collision);

        Vector3 towardsCentre = (transform.position - collision.transform.position).normalized;

        collision.gameObject.transform.position += towardsCentre * kPullSpeedConst * m_SuckSpeed;
    }
    override public void OnTriggerEnter2D(Collider2D collision)
    {
        TogglePhysics(collision.gameObject, true);
    }

    override public void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        TogglePhysics(collision.gameObject, false);
    }

    private void TogglePhysics(GameObject gameObject, bool toggle)
    {
        if (!gameObject.CompareTag("Enemy")) return;
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (!rb) return;

        rb.isKinematic = toggle;
    }
}
