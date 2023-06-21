using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_WalkSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        Vector3 currentVelocity = rigidBody.velocity;

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);

        Vector3 targetVelocity = moveDir * m_WalkSpeed;

        currentVelocity = targetVelocity;

        rigidBody.velocity = currentVelocity;

        transform.GetComponentInChildren<SpriteRenderer>().flipX = rigidBody.velocity.x < 0;
    }
}
