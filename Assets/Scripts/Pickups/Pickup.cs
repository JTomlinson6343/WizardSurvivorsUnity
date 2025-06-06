using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private readonly float kDeceleration = 0.1f;
    private readonly float kPullDist = 2f;
    private readonly float kPullBaseSpeed = 4f;
    private readonly float kLifetime = 60f;

    public float m_PullSpeed;

    private bool m_FinishedDropping = false;
    public bool  m_StartedAttracting = false;
    public bool  m_PickedUp = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || m_PickedUp || !m_FinishedDropping) return;

        OnPickup();
    }

    virtual protected void OnPickup()
    {
        m_PickedUp = true;
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(()=>
        {
            Destroy(gameObject);
        });
    }

    private void Awake()
    {
        Invoke(nameof(DestroySelf), kLifetime);

        m_PullSpeed = kPullBaseSpeed;

        LeanTween.delayedCall(1f, () => { m_FinishedDropping = true; });
    }
    private void Update()
    {
        if (m_FinishedDropping)
        {
            MoveTowardsPlayer();
            return;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Decelerate
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, kDeceleration);
    }

    void MoveTowardsPlayer()
    {
        // If player is not in range, return
        float distToPlayer = Vector2.Distance(transform.position, Player.m_Instance.GetPosition());
        if (distToPlayer > kPullDist * (1f+Player.m_Instance.GetStats().pullDist) && !m_StartedAttracting)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        m_StartedAttracting = true;
        GetComponent<Rigidbody2D>().velocity = Utils.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject) * m_PullSpeed;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
