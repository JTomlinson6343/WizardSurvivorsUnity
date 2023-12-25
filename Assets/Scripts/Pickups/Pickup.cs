using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private readonly float kDeceleration = 0.1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        OnPickup();
    }

    virtual protected void OnPickup()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude <= 0) return;

        // Decelerate
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, kDeceleration);
    }
}
