using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private readonly float kDeceleration = 0.1f;
    private readonly float kPullDist = 2f;
    private readonly float kPullSpeed = 4f;
    private readonly float kLifetime = 60f;

    private bool m_FinishedDropping = false;
    private bool m_StartedAttracting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        OnPickup();
    }

    virtual protected void OnPickup()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        Invoke(nameof(DestroySelf), kLifetime);
    }
    private void Update()
    {
        if (m_FinishedDropping)
        {
            MoveTowardsPlayer();
            return;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // If pickup has stopped moving, set bool to true
        if (rb.velocity.magnitude <= 0.2f) {
            m_FinishedDropping = true;
            return; 
        }

        // Decelerate
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, kDeceleration);
    }

    void MoveTowardsPlayer()
    {
        // If player is not in range, return
        float distToPlayer = Vector2.Distance(transform.position, Player.m_Instance.GetPosition());
        if (distToPlayer > kPullDist && !m_StartedAttracting && StateManager.GetCurrentState() != State.BOSS) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        m_StartedAttracting = true;
        GetComponent<Rigidbody2D>().velocity = GameplayManager.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject) * kPullSpeed;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
