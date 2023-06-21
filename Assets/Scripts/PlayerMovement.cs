using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        transform.GetComponentInChildren<SpriteRenderer>().flipX = rigidBody.velocity.x < 0;
    }
}
