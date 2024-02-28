using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDamageAOE : AOEObject
{
    readonly float kTickrate = 0.25f;
    private void Start()
    {
        m_HitboxDelay = kTickrate;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        OnTargetHit(collision.gameObject);
    }

    override public void OnTriggerEnter2D(Collider2D collision) { }
}
