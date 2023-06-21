using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerReference;
    [SerializeField] private float m_WalkSpeed = 1.0f;

    private void Awake()
    {
        m_PlayerReference = GameObject.Find("Player");
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

        transform.parent.GetComponent<Rigidbody2D>().velocity = moveDir * m_WalkSpeed;
    }
}
