using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_RigidBody;
    [SerializeField] private float m_WalkSpeed = 1.0f;
    private float m_Acceleration = 8.0f;

    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = m_RigidBody.velocity;

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);

        Vector3 targetVelocity = moveDir * m_WalkSpeed;

        //currentVelocity = targetVelocity;
        currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * m_Acceleration;

        m_RigidBody.velocity = currentVelocity;

        SetAnimState(targetVelocity);

        UpdateHealthBar();
    }

    void SetAnimState(Vector3 targetVelocity)
    {
        SpriteRenderer sprite = transform.GetComponentInChildren<SpriteRenderer>();

        if (targetVelocity.x > 0)
        {
            sprite.flipX = false;
        }
        else if (targetVelocity.x < 0)
        {
            sprite.flipX = true;
        }

        if (targetVelocity.magnitude > 0)
        {
            // Object is moving
            m_Animator.SetBool("Moving", true);
            m_Animator.SetBool("Idle", false); // Disable the idle state
        }
        else
        {
            // Object is not moving
            m_Animator.SetBool("Moving", false); // Disable the moving state
            m_Animator.SetBool("Idle", true);
        }
    }

    void UpdateHealthBar()
    {
        Actor actorComponent = gameObject.GetComponent<Actor>();

        float health = actorComponent.GetHealthAsRatio();

        BasicBar bar = gameObject.GetComponentInChildren<BasicBar>();

        bar.UpdateSize(health);
    }
}
