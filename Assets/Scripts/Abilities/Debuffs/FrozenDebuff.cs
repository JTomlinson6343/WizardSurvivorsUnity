using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FrozenDebuff : Debuff_old
{
    protected override void DebuffBehaviour()
    {
        base.DebuffBehaviour();

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        if (!rb) return;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        GetComponentInChildren<Animator>().enabled = false;
    }

    protected override void EndDebuff()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponentInChildren<Animator>().enabled = true;
        base.EndDebuff();
    }
    protected override void TickRoutine() { }
}
