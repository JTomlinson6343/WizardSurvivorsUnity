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
        m_Animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerReference = Player.m_Instance.gameObject;
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

        SetAnimState(targetVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        Vector3 objectPos = otherObject.transform.position;
        Vector3 currentPos = gameObject.transform.position;

        Vector3 moveDir = (objectPos - currentPos).normalized;

        //Vector3 currentVelocity = m_RigidBody.velocity;

        //currentVelocity -= moveDir * 1.0f;

        if (otherObject.name == "Player")
        {
            Actor actorComponent = otherObject.GetComponent<Actor>();

            bool validHit = actorComponent.TakeDamage(1.0f);

            if (validHit)
            {
                Rigidbody2D playerBody = otherObject.GetComponent<Rigidbody2D>();

                playerBody.velocity += new Vector2(moveDir.x, moveDir.y) * 15.0f;
            }
        }
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
}
