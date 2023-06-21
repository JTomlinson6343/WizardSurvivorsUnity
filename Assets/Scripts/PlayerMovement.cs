using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_RigidBody;
    [SerializeField] private float m_WalkSpeed = 1.0f;

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

        currentVelocity = targetVelocity;
        //currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * 5.0f;

        m_RigidBody.velocity = currentVelocity;

        transform.GetComponentInChildren<SpriteRenderer>().flipX = targetVelocity.x < 0;

        SetAnimState();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 objectPos = collision.gameObject.transform.position;
        Vector3 currentPos = gameObject.transform.position;

        Vector3 moveDir = (objectPos - currentPos).normalized;

        Vector3 currentVelocity = m_RigidBody.velocity;

        currentVelocity -= moveDir * 1.0f;

        //m_RigidBody.velocity = currentVelocity;
    }

    void SetAnimState()
    {
        Vector3 currentVelocity = m_RigidBody.velocity;

        if (currentVelocity.magnitude > 0)
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
}
