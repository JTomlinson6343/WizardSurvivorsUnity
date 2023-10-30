using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
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

    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING)
        {
            m_RigidBody.velocity = Vector3.zero;
            return;
        }

        Vector3 currentPos = gameObject.transform.position;
        if (Player.m_Instance != null )
        {
            Vector3 playerPos = Player.m_Instance.transform.position;
            Vector3 moveDir = (playerPos - currentPos).normalized;
            Vector3 targetVelocity = moveDir * m_WalkSpeed;
            Vector3 currentVelocity = m_RigidBody.velocity;

            currentVelocity += (targetVelocity - currentVelocity) * Time.deltaTime * 5.0f;

            m_RigidBody.velocity = currentVelocity;

            SetAnimState(targetVelocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        Vector2 objectPos = otherObject.transform.position;
        Vector2 currentPos = gameObject.transform.position;

        Vector2 moveDir = (objectPos - currentPos).normalized;

        //Vector3 currentVelocity = m_RigidBody.velocity;

        //currentVelocity -= moveDir * 1.0f;

        if (otherObject.GetComponent<Player>() != null)
        {
            Enemy enemy = GetComponent<Enemy>();
            DamageInstanceData data = new(gameObject, otherObject);
            data.amount = enemy.m_ContactDamage;
            data.damageType = DamageType.Physical;
            data.doDamageNumbers = false;

            if (DamageManager.m_Instance.DamageInstance(data, transform.position) >= DamageOutput.validHit)
            {
                Rigidbody2D playerBody = otherObject.GetComponent<Rigidbody2D>();

                playerBody.velocity += GetComponent<Rigidbody2D>().velocity.normalized * 45.0f;
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
