using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerReference;
    [SerializeField] private Rigidbody2D m_RigidBody;
    [SerializeField] private float m_WalkSpeed = 1.0f;

    private Animator m_Animator;

    private void Awake()
    {
        m_PlayerReference = GameObject.Find("Player");
        m_Animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = gameObject.transform.position;
        Vector3 playerPos = m_PlayerReference.transform.position;
        Vector3 moveDir = (playerPos - currentPos).normalized;

        Vector3 targetVelocity = moveDir * m_WalkSpeed;
        Vector3 currentVelocity = m_RigidBody.velocity;

        currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * 5.0f;

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

        if (currentVelocity.magnitude > 0.3f)
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
